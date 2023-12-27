using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarParecerConclusivoAlunoCommandHandler : IRequestHandler<GerarParecerConclusivoAlunoCommand, ParecerConclusivoDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;

        public GerarParecerConclusivoAlunoCommandHandler(IMediator mediator, IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }

        public async Task<ParecerConclusivoDto> Handle(GerarParecerConclusivoAlunoCommand request,
            CancellationToken cancellationToken)
        {
            var conselhoClasseAluno = request.ConselhoClasseAluno;
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(conselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.CodigoTurma));
            var alunosEol =
                await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma, consideraInativos: true));
            var alunoNaTurma = alunosEol.FirstOrDefault(a => a.CodigoAluno == conselhoClasseAluno.AlunoCodigo);
            bool historico = turma.Historica;
            var emAprovacao = await EnviarParaAprovacao(turma);

            if (alunoNaTurma.NaoEhNulo())
                historico = alunoNaTurma.Inativo;

            // Se não possui notas de fechamento nem de conselho retorna um Dto vazio
            if (!await VerificaNotasTodosComponentesCurriculares(conselhoClasseAluno.AlunoCodigo, turma, null))
                return new ParecerConclusivoDto();

            var pareceresDaTurma = await ObterPareceresDaTurma(turma);
            var parecerConclusivo =
                await mediator.Send(new ObterParecerConclusivoAlunoQuery(conselhoClasseAluno.AlunoCodigo,
                    turma.CodigoTurma, pareceresDaTurma));

            if (parecerConclusivo.Id == conselhoClasseAluno.ConselhoClasseParecerId)
                return new ParecerConclusivoDto()
                {
                    Id = parecerConclusivo?.Id ?? 0,
                    Nome = parecerConclusivo?.Nome,
                    EmAprovacao = false
                };

            if (emAprovacao)
                emAprovacao = await GerarWFAprovacao(conselhoClasseAluno, parecerConclusivo.Id, pareceresDaTurma, request.UsuarioSolicitanteId);
            else
            {
                var bimestre = conselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar.EhNulo() ? (int?)null : conselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar.Bimestre;
                conselhoClasseAluno.ConselhoClasseParecerId = parecerConclusivo.Id;
                var persistirParecerConclusivoDto = new PersistirParecerConclusivoDto()
                {
                    ConselhoClasseAlunoId = conselhoClasseAluno.Id,
                    ConselhoClasseAlunoCodigo = conselhoClasseAluno.AlunoCodigo,
                    ParecerConclusivoId = parecerConclusivo.Id,
                    TurmaId = turma.Id,
                    TurmaCodigo = turma.CodigoTurma,
                    Bimestre = bimestre,
                    AnoLetivo = turma.AnoLetivo
                };
                await mediator.Send(new PersistirParecerConclusivoCommand(persistirParecerConclusivoDto));
            }

            return new ParecerConclusivoDto()
            {
                Id = parecerConclusivo?.Id ?? 0,
                Nome = parecerConclusivo?.Nome,
                EmAprovacao = emAprovacao
            };
        }

        private async Task<bool> GerarWFAprovacao(ConselhoClasseAluno conselhoClasseAluno, long parecerConclusivoId, IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma, long usuarioSolicitanteId)
        {
            if (parecerConclusivoId == conselhoClasseAluno.ConselhoClasseParecerId)
                return false;

            var turma = conselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma;
            var parecerAnterior = pareceresDaTurma.FirstOrDefault(a => a.Id == conselhoClasseAluno.ConselhoClasseParecerId)?.Nome;
            var parecerNovo = pareceresDaTurma.FirstOrDefault(a => a.Id == parecerConclusivoId).Nome;

            var pareceresEmAprovacaoAtual = await mediator.Send(new ObterParecerConclusivoEmAprovacaoPorConselhoClasseAlunoQuery(conselhoClasseAluno.Id));
            if (!pareceresEmAprovacaoAtual.Any(parecer => parecer.ConselhoClasseParecerId == parecerConclusivoId))
                await mediator.Send(new GerarWFAprovacaoParecerConclusivoCommand(conselhoClasseAluno.Id,
                                                                 turma,
                                                                 conselhoClasseAluno.AlunoCodigo,
                                                                 parecerConclusivoId,
                                                                 parecerAnterior,
                                                                 parecerNovo,
                                                                 usuarioSolicitanteId,
                                                                 conselhoClasseAluno.ConselhoClasseParecerId));

            return true;
        }

        private async Task<bool> EnviarParaAprovacao(Turma turma)
        {
            return turma.AnoLetivo < DateTime.Today.Year
                && await ParametroAprovacaoAtivo(turma.AnoLetivo);
        }

        private async Task<bool> ParametroAprovacaoAtivo(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoParecerConclusivo, anoLetivo));
            if (parametro.EhNulo())
                throw new NegocioException($"Não localizado parametro de aprovação de alteração de parecer conclusivo para o ano {anoLetivo}");

            return parametro.Ativo;
        }

        private async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterPareceresDaTurma(Turma turma)
        {
            var pareceresConclusivos = await mediator.Send(new ObterPareceresConclusivosPorTurmaQuery(turma));
            if (pareceresConclusivos.EhNulo() || !pareceresConclusivos.Any())
                throw new NegocioException("Não foram encontrados pareceres conclusivos para a turma!");

            return pareceresConclusivos;
        }

        public async Task<bool> VerificaNotasTodosComponentesCurriculares(string alunoCodigo, Turma turma, long? periodoEscolarId)
        {
            int bimestre;
            long[] conselhosClassesIds;
            string[] turmasCodigos;
            var turmasItinerarioEnsinoMedio = (await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance)).ToList();

            if ((turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma)) && !(periodoEscolarId.EhNulo() && turma.EhEJA()))
            {
                var tiposTurmasParaConsulta = new List<int> { (int)turma.TipoTurma };

                tiposTurmasParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());
                tiposTurmasParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id));

                var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, 1));

                if (periodoEscolar.NaoEhNulo() && periodoEscolar.Any())
                {
                    var turmasCodigosEOL = await mediator
                        .Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, tiposTurmasParaConsulta, dataReferencia: periodoEscolar.First(x => x.Bimestre == 1).PeriodoInicio, ueCodigo: turma.Ue.CodigoUe, semestre: turma.Semestre != 0 ? turma.Semestre : null));

                    if (turmasCodigosEOL.NaoEhNulo() && turmasCodigosEOL.Any())
                    {
                        var turmasComMatriculasValidas = await mediator.Send(new ObterTurmasComMatriculasValidasQuery(alunoCodigo, turmasCodigosEOL, DateTimeExtension.HorarioBrasilia().Date, DateTimeExtension.HorarioBrasilia().Date));
                        if (turmasComMatriculasValidas.Any())
                            turmasCodigosEOL = turmasComMatriculasValidas.ToArray();
                    }

                    var turmasEOL = turmasCodigosEOL != null && turmasCodigosEOL.Any() ? await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigosEOL)) : null;

                    turmasCodigosEOL = turmasEOL.NaoEhNulo() && turmasEOL.Any() ? turmasEOL.Where(x => x.Ano == turma.Ano).Select(x => x.CodigoTurma).ToArray() : null;

                    if (turma.Historica == true && turmasCodigosEOL.NaoEhNulo() && turmasCodigosEOL.Any())
                    {
                        var turmasCodigosHistorico = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigosEOL));

                        if (turmasCodigosHistorico.NaoEhNulo() && turmasCodigosHistorico.Any(x => x.EhTurmaHistorica))
                        {
                            turmasCodigos = turmasCodigosEOL;
                            turmasCodigos = turmasCodigos
                                .Concat(new string[] { turma.CodigoTurma }).ToArray();
                        }
                        else
                            turmasCodigos = new string[] { turma.CodigoTurma };
                    }
                    else
                        turmasCodigos = turmasCodigosEOL.NaoEhNulo() && turmasCodigosEOL.Any() ? turmasCodigosEOL
                            .Concat(new string[] { turma.CodigoTurma }).ToArray() : new string[] { turma.CodigoTurma };
                }
                else turmasCodigos = new string[] { turma.CodigoTurma };
            }
            else turmasCodigos = new string[] { turma.CodigoTurma };

            turmasCodigos = turmasCodigos.Distinct().ToArray();

            if (periodoEscolarId.HasValue)
            {
                var periodoEscolar = await mediator
                    .Send(new ObterPeriodoEscolarePorIdQuery(periodoEscolarId.Value));

                if (periodoEscolar.EhNulo())
                    throw new NegocioException("Não foi possível localizar o período escolar");

                bimestre = periodoEscolar.Bimestre;

                conselhosClassesIds = await mediator
                    .Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos, periodoEscolar?.Id));
            }
            else
            {
                bimestre = 0;
                conselhosClassesIds = new long[0];
            }

            var notasParaVerificar = new List<NotaConceitoBimestreComponenteDto>();
            if (conselhosClassesIds.NaoEhNulo())
            {
                foreach (var conselhosClassesId in conselhosClassesIds)
                {
                    var notasParaAdicionar = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhosClassesId, alunoCodigo, bimestre));

                    notasParaVerificar.AddRange(notasParaAdicionar);
                }
            }

            if (periodoEscolarId.HasValue)
                notasParaVerificar.AddRange(await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigos, alunoCodigo, bimestre)));
            else
            {
                var todasAsNotas = await mediator.Send(new ObterNotasFinaisBimestresAlunoQuery(turmasCodigos, alunoCodigo));

                if (todasAsNotas.NaoEhNulo() && todasAsNotas.Any())
                    notasParaVerificar.AddRange(todasAsNotas.Where(a => a.Bimestre.EhNulo()));
            }

            var componentesCurriculares = await ObterComponentesTurmas(turmasCodigos, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares);
            var disciplinasDaTurma = await mediator
                .Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurriculares.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray()));

            // Checa se todas as disciplinas da turma receberam nota
            var disciplinasLancamNota = disciplinasDaTurma.Where(c => c.LancaNota && c.GrupoMatrizNome.NaoEhNulo());
            foreach (var componenteCurricular in disciplinasLancamNota)
            {
                if (!notasParaVerificar.Any(c => c.ComponenteCurricularCodigo == componenteCurricular.CodigoComponenteCurricular))
                    return false;
            }

            return true;
        }

        private async Task<IEnumerable<DisciplinaDto>> ObterComponentesTurmas(string[] turmasCodigo, bool ehEnsinoEspecial, int turnoParaComponentesCurriculares)
        {
            var componentesTurma = new List<DisciplinaDto>();
            Usuario usuarioAtual = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigo, usuarioAtual.PerfilAtual, usuarioAtual.Login, ehEnsinoEspecial, turnoParaComponentesCurriculares));
            if (componentesCurriculares.NaoEhNulo() && componentesCurriculares.Any())
                componentesTurma.AddRange(componentesCurriculares);
            else throw new NegocioException("Não localizado disciplinas para a turma no EOL!");

            return componentesTurma;
        }
    }
}

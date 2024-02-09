using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarParecerConclusivoAlunoCommandHandler : IRequestHandler<GerarParecerConclusivoAlunoCommand, ParecerConclusivoDto>
    {
        private readonly IMediator mediator;

        public GerarParecerConclusivoAlunoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ParecerConclusivoDto> Handle(GerarParecerConclusivoAlunoCommand request, CancellationToken cancellationToken)
        {
            var conselhoClasseAluno = request.ConselhoClasseAluno;
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(conselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.CodigoTurma), cancellationToken);
            var emAprovacao = await EnviarParaAprovacao(turma);
            var periodoEscolar = await ObterUltimoBimestreTurma(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre);
            var turmaTecnica = turma.ModalidadeCodigo == Modalidade.Medio && await mediator.Send(new TurmaEhTecnicaQuery(turma, periodoEscolar?.PeriodoInicio ?? DateTimeExtension.HorarioBrasilia()), cancellationToken);
            string[] turmasCodigos = await DefinirTurmasCodigos(conselhoClasseAluno.AlunoCodigo, turma, periodoEscolar, turmaTecnica);

            // Se não possui notas de fechamento nem de conselho retorna um Dto vazio
            if (turma.EhCELP() || !await VerificaNotasTodosComponentesCurriculares(conselhoClasseAluno.AlunoCodigo, turma, null, turmasCodigos))
                return new ParecerConclusivoDto();

            var pareceresDaTurma = await ObterPareceresDaTurma(turma, turmasCodigos, turmaTecnica);
            var parecerConclusivo =
                await mediator.Send(new ObterParecerConclusivoAlunoQuery(conselhoClasseAluno.AlunoCodigo,
                    turma.CodigoTurma, pareceresDaTurma), cancellationToken);

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
                await mediator.Send(new PersistirParecerConclusivoCommand(persistirParecerConclusivoDto), cancellationToken);
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
                await mediator.Send(new GerarWFAprovacaoParecerConclusivoCommand(conselhoClasseAluno,
                                                                                 turma,
                                                                                 parecerConclusivoId,
                                                                                 parecerAnterior,
                                                                                 parecerNovo,
                                                                                 usuarioSolicitanteId));

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

        private async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterPareceresDaTurma(Turma turmaSelecionada, string[] turmasCodigos, bool turmaTecnica = false)
        {
            var turmaReferencia = turmaSelecionada;

            if (turmaTecnica)
            {
                var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigos));
                turmaReferencia = turmas.FirstOrDefault(t => t.EhTurmaRegular() && t.CodigoTurma != turmaSelecionada.CodigoTurma) ?? turmaSelecionada;
            }

            var pareceresConclusivos = await mediator.Send(new ObterPareceresConclusivosPorTurmaQuery(turmaReferencia));

            if (pareceresConclusivos.EhNulo() || !pareceresConclusivos.Any())
                throw new NegocioException("Não foram encontrados pareceres conclusivos para a turma!");

            return pareceresConclusivos;
        }

        public async Task<bool> VerificaNotasTodosComponentesCurriculares(string alunoCodigo, Turma turmaSelecionada, long? periodoEscolarId, params string[] turmasCodigos)
        {
            int bimestre;
            long[] conselhosClassesIds;

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
                conselhosClassesIds = Enumerable.Empty<long>().ToArray();
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

            var componentesCurriculares = await ObterComponentesTurmas(turmasCodigos, turmaSelecionada.EnsinoEspecial, turmaSelecionada.TurnoParaComponentesCurriculares);
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

        private async Task<string[]> DefinirTurmasCodigos(string alunoCodigo, Turma turma, PeriodoEscolar periodoEscolar, bool turmaTecnica = false)
        {
            string[] turmasCodigos;
            var turmasItinerarioEnsinoMedio = (await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance)).ToList();

            if ((turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma)) && !(periodoEscolar.EhNulo() && turma.EhEJA()))
            {
                var tiposTurmasParaConsulta = new List<int> { (int)turma.TipoTurma };

                tiposTurmasParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());
                tiposTurmasParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id));

                if (periodoEscolar.NaoEhNulo())
                {
                    var turmasCodigosEOL = await mediator
                        .Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, tiposTurmasParaConsulta, dataReferencia: periodoEscolar.PeriodoFim, ueCodigo: turma.Ue.CodigoUe, semestre: turma.Semestre != 0 ? turma.Semestre : null));

                    if (turmasCodigosEOL.NaoEhNulo() && turmasCodigosEOL.Any())
                    {
                        var turmasComMatriculasValidas = await mediator.Send(new ObterTurmasComMatriculasValidasQuery(alunoCodigo, turmasCodigosEOL, DateTimeExtension.HorarioBrasilia().Date, DateTimeExtension.HorarioBrasilia().Date));
                        if (turmasComMatriculasValidas.Any())
                            turmasCodigosEOL = turmasComMatriculasValidas.ToArray();
                    }

                    var turmasEOL = turmasCodigosEOL.NaoEhNulo() && turmasCodigosEOL.Any() ? await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigosEOL)) : null;

                    if (!turmaTecnica)
                        turmasCodigosEOL = turmasEOL.NaoEhNulo() && turmasEOL.Any() ? turmasEOL.Where(x => x.Ano == turma.Ano).Select(x => x.CodigoTurma).ToArray() : null;

                    if (turma.Historica && turmasCodigosEOL.NaoEhNulo() && turmasCodigosEOL.Any())
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

            return turmasCodigos.Distinct().ToArray();
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

        private async Task<PeriodoEscolar> ObterUltimoBimestreTurma(Modalidade modalidadeTurma, int anoLetivoTurma, int semestreTurma)
            => (await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(modalidadeTurma, anoLetivoTurma, semestreTurma))).OrderBy(p => p.Bimestre).LastOrDefault();
    }
}

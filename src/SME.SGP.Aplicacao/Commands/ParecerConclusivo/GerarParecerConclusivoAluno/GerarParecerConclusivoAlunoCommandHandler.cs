﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
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
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;

        public GerarParecerConclusivoAlunoCommandHandler(IMediator mediator, IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }

        public async Task<ParecerConclusivoDto> Handle(GerarParecerConclusivoAlunoCommand request, CancellationToken cancellationToken)
        {
            var conselhoClasseAluno = request.ConselhoClasseAluno;
            var turma = conselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma;
                        
            // Se não possui notas de fechamento nem de conselho retorna um Dto vazio
            if (!await VerificaNotasTodosComponentesCurriculares(conselhoClasseAluno.AlunoCodigo, turma, null, turma.Historica))
                return new ParecerConclusivoDto();

            var pareceresDaTurma = await ObterPareceresDaTurma(turma.Id);
            var parecerConclusivo = await mediator.Send(new ObterParecerConclusivoAlunoQuery(conselhoClasseAluno.AlunoCodigo, turma.CodigoTurma, pareceresDaTurma));

            var emAprovacao = await EnviarParaAprovacao(turma);
            if (await EnviarParaAprovacao(turma))
                await GerarWFAprovacao(conselhoClasseAluno, parecerConclusivo.Id, pareceresDaTurma, request.UsuarioSolicitanteId);
            else
            {
                var bimestre = conselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar == null ? (int?)null : conselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar.Bimestre;
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

        private async Task GerarWFAprovacao(ConselhoClasseAluno conselhoClasseAluno, long parecerConclusivoId, IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma, long usuarioSolicitanteId)
        {
            if (parecerConclusivoId == conselhoClasseAluno.ConselhoClasseParecerId)
                return;

            var turma = conselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma;
            var parecerAnterior = pareceresDaTurma.FirstOrDefault(a => a.Id == conselhoClasseAluno.ConselhoClasseParecerId)?.Nome;
            var parecerNovo = pareceresDaTurma.FirstOrDefault(a => a.Id == parecerConclusivoId).Nome;

            await mediator.Send(new GerarWFAprovacaoParecerConclusivoCommand(conselhoClasseAluno.Id,
                                                                             turma,
                                                                             conselhoClasseAluno.AlunoCodigo,
                                                                             parecerConclusivoId,
                                                                             parecerAnterior,
                                                                             parecerNovo,
                                                                             usuarioSolicitanteId));
        }

        private async Task<bool> EnviarParaAprovacao(Turma turma)
        {
            return turma.AnoLetivo < DateTime.Today.Year
                && await ParametroAprovacaoAtivo(turma.AnoLetivo);
        }

        private async Task<bool> ParametroAprovacaoAtivo(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoParecerConclusivo, anoLetivo));
            if (parametro == null)
                throw new NegocioException($"Não localizado parametro de aprovação de alteração de parecer conclusivo para o ano {anoLetivo}");

            return parametro.Ativo;
        }

        private async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterPareceresDaTurma(long turmaId)
        {
            var pareceresConclusivos = await mediator.Send(new ObterPareceresConclusivosPorTurmaQuery(turmaId));
            if (pareceresConclusivos == null || !pareceresConclusivos.Any())
                throw new NegocioException("Não foram encontrados pareceres conclusivos para a turma!");

            return pareceresConclusivos;
        }

        public async Task<bool> VerificaNotasTodosComponentesCurriculares(string alunoCodigo, Turma turma, long? periodoEscolarId, bool? historico = false)
        {
            int bimestre;
            long[] conselhosClassesIds;
            string[] turmasCodigos;

            if (turma.DeveVerificarRegraRegulares())
            {
                turmasCodigos = await mediator
                    .Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, turma.ObterTiposRegularesDiferentes(), historico));

                turmasCodigos = turmasCodigos
                    .Concat(new string[] { turma.CodigoTurma }).ToArray();
            }
            else turmasCodigos = new string[] { turma.CodigoTurma };


            if (periodoEscolarId.HasValue)
            {
                var periodoEscolar = await mediator
                    .Send(new ObterPeriodoEscolarePorIdQuery(periodoEscolarId.Value));

                if (periodoEscolar == null)
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
            if (conselhosClassesIds != null)
            {
                foreach (var conselhosClassesId in conselhosClassesIds)
                {
                    var notasParaAdicionar = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhosClassesId, alunoCodigo));

                    notasParaVerificar.AddRange(notasParaAdicionar);
                }
            }

            if (periodoEscolarId.HasValue)
                notasParaVerificar.AddRange(await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigos, alunoCodigo, bimestre)));
            else
            {
                var todasAsNotas = await mediator.Send(new ObterNotasFinaisBimestresAlunoQuery(turmasCodigos, alunoCodigo));

                if (todasAsNotas != null && todasAsNotas.Any())
                    notasParaVerificar.AddRange(todasAsNotas.Where(a => a.Bimestre == null));
            }

            var componentesCurriculares = await ObterComponentesTurmas(turmasCodigos, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares);
            var disciplinasDaTurma = await mediator
                .Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurriculares.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray()));

            // Checa se todas as disciplinas da turma receberam nota
            var disciplinasLancamNota = disciplinasDaTurma.Where(c => c.LancaNota && c.GrupoMatrizNome != null);
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
            Usuario usuarioAtual = await mediator.Send(new ObterUsuarioLogadoQuery());

            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigo, usuarioAtual.PerfilAtual, usuarioAtual.Login, ehEnsinoEspecial, turnoParaComponentesCurriculares));
            if (componentesCurriculares != null && componentesCurriculares.Any())
                componentesTurma.AddRange(componentesCurriculares);
            else throw new NegocioException("Não localizado disciplinas para a turma no EOL!");

            return componentesTurma;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries
{
    public class VerificaNotasTodosComponentesCurricularesQueryHandler : IRequestHandler<VerificaNotasTodosComponentesCurricularesQuery, bool>
    {
        private readonly IMediator mediator;
        private const string PRIMEIRO_ANO_EM = "1";

        public VerificaNotasTodosComponentesCurricularesQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificaNotasTodosComponentesCurricularesQuery request, CancellationToken cancellationToken)
        {
            string[] turmasCodigos = await ObterTurmasAluno(request, cancellationToken);
            var conselhosClassesIds = await mediator.Send(new ObterConselhoClasseIdsPorTurmaEBimestreQuery(turmasCodigos, request.Bimestre), cancellationToken);

            var notasParaVerificar = new List<NotaConceitoBimestreComponenteDto>();

            if (conselhosClassesIds.NaoEhNulo())
            {
                foreach (var conselhosClassesId in conselhosClassesIds)
                {
                    var notasParaAdicionar = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhosClassesId, request.AlunoCodigo, request.Bimestre ?? 0), cancellationToken);
                    notasParaVerificar.AddRange(notasParaAdicionar);
                }
            }

            var todasAsNotas = (await mediator.Send(new ObterNotasFinaisBimestresAlunoQuery(turmasCodigos, request.AlunoCodigo), cancellationToken)).ToList();

            if (todasAsNotas.Any())
            {
                var bimestre = request.Bimestre == 0 ? null : request.Bimestre;

                notasParaVerificar.AddRange(todasAsNotas.Where(a => a.Bimestre == bimestre));
            }

            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(request.Turma.AnoLetivo, request.Turma.ModalidadeTipoCalendario, request.Turma.Semestre));
            if (tipoCalendario.EhNulo())
                throw new NegocioException(MensagemNegocioTipoCalendario.TIPO_CALENDARIO_NAO_ENCONTRADO);
            var periodosLetivos = (await mediator
                .Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendario.Id))).ToList();
                        

            if (periodosLetivos.EhNulo() || !periodosLetivos.Any())
                throw new NegocioException(MensagemNegocioPeriodo.NAO_FORAM_ENCONTRADOS_PERIODOS_TIPO_CALENDARIO);

            var periodoInicio = request.PeriodoEscolar?.PeriodoInicio ?? periodosLetivos.OrderBy(pl => pl.Bimestre).First().PeriodoInicio;
            var periodoFim = request.PeriodoEscolar?.PeriodoFim ?? periodosLetivos.OrderBy(pl => pl.Bimestre).Last().PeriodoFim;
            var bimestrePeriodo = request.PeriodoEscolar?.Bimestre ?? (int)Dominio.Bimestre.Final;

            var turmasComMatriculasValidas = await mediator.Send(new ObterTurmasComMatriculasValidasPeriodoQuery(request.AlunoCodigo,
                                                                                                                    request.Turma.EhTurmaInfantil,
                                                                                                                    bimestrePeriodo,
                                                                                                                    tipoCalendario.Id,
                                                                                                                    turmasCodigos.ToArray(),
                                                                                                                    periodoInicio,
                                                                                                                    periodoFim));
            if (turmasComMatriculasValidas.Any())
                turmasCodigos = turmasComMatriculasValidas.ToArray();
            //var matriculasDoAluno = await mediator.Send(new ObterMatriculasTurmaPorCodigoAlunoQuery(request.AlunoCodigo, dataAula: request.PeriodoEscolar?.PeriodoFim, request.Turma.AnoLetivo));
            //turmasCodigos = await DefinirTurmasConsideradasDeAcordoComMatricula(matriculasDoAluno, request.PeriodoEscolar, turmasCodigos);

            var componentesCurriculares = await ObterComponentesTurmas(turmasCodigos, request.Turma.EnsinoEspecial, request.Turma.TurnoParaComponentesCurriculares);
            var disciplinasDaTurma = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(componentesCurriculares.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray(), codigoTurma: request.Turma.CodigoTurma), cancellationToken);

            // Checa se todas as disciplinas da turma receberam nota
            var disciplinasLancamNota = disciplinasDaTurma.Where(c => c.LancaNota && c.GrupoMatrizNome.NaoEhNulo());
            return disciplinasLancamNota.All(componenteCurricular => notasParaVerificar.Any(c => c.ComponenteCurricularCodigo == componenteCurricular.CodigoComponenteCurricular));
        }

        private async Task<string[]> ObterTurmasAluno(VerificaNotasTodosComponentesCurricularesQuery request, CancellationToken cancellationToken)
        {
            string[] turmasCodigos = new[] { request.Turma.CodigoTurma };
            var turmasItinerarioEnsinoMedio = (await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance, cancellationToken)).ToList();
            if (request.Turma.DeveVerificarRegraRegulares() 
                || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)request.Turma.TipoTurma))
            {
                var tiposTurmas = new List<int> { (int)request.Turma.TipoTurma };
                tiposTurmas.AddRange(request.Turma.ObterTiposRegularesDiferentes());
                tiposTurmas.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposTurmas.All(x => x != c)));

                var turmasCodigosEol = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(request.Turma.AnoLetivo, request.AlunoCodigo, tiposTurmas,
                   dataReferencia: request.PeriodoEscolar?.PeriodoFim, semestre: request.Turma.EhEJA() ? request.Turma.Semestre : null), cancellationToken);

                if (request.Historico.HasValue && request.Historico.Value)
                {
                    var turmasCodigosHistorico = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigosEol), cancellationToken);
                    if (turmasCodigosHistorico.Any(x => x.EhTurmaHistorica))
                    {
                        turmasCodigos = turmasCodigosEol;
                        turmasCodigos = !turmasCodigosEol.Contains(request.Turma.CodigoTurma) ? turmasCodigos.Concat(new[] { request.Turma.CodigoTurma }).ToArray() : turmasCodigosEol;
                    }
                }
                else
                    turmasCodigos = !turmasCodigosEol.Contains(request.Turma.CodigoTurma) ? turmasCodigosEol.Concat(new[] { request.Turma.CodigoTurma }).ToArray() : turmasCodigosEol;

                if (turmasCodigos.Any())
                {
                    var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigos.ToArray()));
                    if (turmas.Select(t => t.TipoTurma).Distinct().Count() == 1 
                        && request.Turma.ModalidadeCodigo != Modalidade.Medio)
                        turmasCodigos = new string[] { request.Turma.CodigoTurma };
                    else if (ValidaPossibilidadeMatricula2TurmasRegularesNovoEM(turmas, request.Turma))
                        turmasCodigos = new string[] { request.Turma.CodigoTurma };
                }
            };
            return turmasCodigos;
        }

        private async Task<string[]> DefinirTurmasConsideradasDeAcordoComMatricula(IEnumerable<AlunoPorTurmaResposta> matriculasDoAluno, Dominio.PeriodoEscolar periodoEscolar, string[] turmasCodigos)
        {
            if (periodoEscolar.NaoEhNulo())
            {
                var turmasCodigosComMatriculasValidas = new List<string>();
                foreach (string codTurma in turmasCodigos)
                {
                    Func<Task<Turma>> fncInstanciarTurma = async () => await mediator.Send(new ObterTurmaPorCodigoQuery(codTurma));
                    var turmasConsideradas = (from m in matriculasDoAluno
                                              where ((m.Ativo && m.DataMatricula.Date < periodoEscolar.PeriodoFim) ||
                                                     (m.Inativo && m.DataMatricula.Date < periodoEscolar.PeriodoFim && m.DataSituacao.Date >= periodoEscolar.PeriodoInicio)) &&
                                                      m.CodigoSituacaoMatricula != SituacaoMatriculaAluno.VinculoIndevido
                                                      && !(m.PossuiSituacaoDispensadoTurmaEdFisica(fncInstanciarTurma)
                                                      && m.CodigoTurma.ToString() == codTurma)
                                              select m.CodigoTurma.ToString()).Distinct();
                    if (turmasConsideradas.Any())
                        turmasCodigosComMatriculasValidas.Add(codTurma);
                }
                if (turmasCodigosComMatriculasValidas.Any())
                    return turmasCodigosComMatriculasValidas.ToArray();
            }
            else
            {
                var matriculasAtivas = matriculasDoAluno
                    .Where(x => x.Ativo && turmasCodigos.Contains(x.CodigoTurma.ToString()));

                if (!matriculasAtivas.Any())
                    return turmasCodigos;

                turmasCodigos = matriculasAtivas
                    .OrderByDescending(x => x.DataSituacao)?
                        .GroupBy(x => x.CodigoTurma)
                            .Select(x => x.First().CodigoTurma.ToString()).ToArray();
            }

            return turmasCodigos;
        }

        private async Task<IEnumerable<DisciplinaDto>> ObterComponentesTurmas(string[] turmasCodigo, bool ehEnsinoEspecial, int turnoParaComponentesCurriculares)
        {
            var componentesTurma = new List<DisciplinaDto>();
            var usuarioAtual = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigo, usuarioAtual.PerfilAtual, usuarioAtual.Login, ehEnsinoEspecial, turnoParaComponentesCurriculares));
            if (componentesCurriculares.NaoEhNulo() && componentesCurriculares.Any())
                componentesTurma.AddRange(componentesCurriculares);
            else throw new NegocioException(MensagemNegocioEOL.NAO_LOCALIZADO_DISCIPLINAS_TURMA_EOL);

            return componentesTurma;
        }

        private bool ValidaPossibilidadeMatricula2TurmasRegularesNovoEM(IEnumerable<Turma> turmasAluno, Turma turmaFiltro)
             => turmasAluno.Select(t => t.TipoTurma).Distinct().Count() == 1 && turmaFiltro.ModalidadeCodigo == Modalidade.Medio && (turmaFiltro.AnoLetivo < 2021 || turmaFiltro.Ano == PRIMEIRO_ANO_EM);

    }
}
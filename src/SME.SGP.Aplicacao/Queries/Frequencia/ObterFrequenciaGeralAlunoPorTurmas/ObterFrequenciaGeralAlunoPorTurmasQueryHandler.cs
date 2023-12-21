using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralAlunoPorTurmasQueryHandler : IRequestHandler<ObterFrequenciaGeralAlunoPorTurmasQuery, string>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IMediator mediator;

        public ObterFrequenciaGeralAlunoPorTurmasQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo,
                                                              IMediator mediator)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Handle(ObterFrequenciaGeralAlunoPorTurmasQuery request, CancellationToken cancellationToken)
        {
            var frequenciaAlunoPeriodos = new List<FrequenciaAluno>();
            var disciplinasAluno = new string[] { };

            frequenciaAlunoPeriodos.AddRange(await repositorioFrequenciaAlunoDisciplinaPeriodo
                .ObterFrequenciaGeralAlunoPorTurmas(request.CodigoAluno, request.CodigosTurmas, request.TipoCalendarioId));

            var bimestres = await mediator.Send(new ObterBimestresPorTipoCalendarioQuery(request.TipoCalendarioId));
            var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(request.CodigosTurmas));
            var periodoEscolarAtual = await mediator.Send(new ObterPeriodoEscolarAtualPorTurmaQuery(turmas.FirstOrDefault(), DateTime.Now));
            var aulasComponentesTurmas = new List<TurmaDataAulaComponenteQtdeAulasDto>();

            foreach (var matricula in request.MatriculasAlunoNaTurma.OrderBy(m => m.dataMatricula))
            {
                aulasComponentesTurmas.AddRange(await mediator
                    .Send(new ObterAulasTurmaEBimestreEComponenteCurricularQuery(request.CodigosTurmas,
                                                                                 new string[] { request.CodigoAluno },
                                                                                 request.TipoCalendarioId,
                                                                                 new string[] { },
                                                                                 bimestres.ToArray(),
                                                                                 matricula.dataMatricula,
                                                                                 !matricula.inativo && periodoEscolarAtual.NaoEhNulo() && periodoEscolarAtual.Bimestre == bimestres.Last() ? periodoEscolarAtual.PeriodoFim : matricula.dataSituacao)));
            }

            var frequenciaAluno = new FrequenciaAluno()
            {
                TotalAulas = frequenciaAlunoPeriodos.Sum(f => f.TotalAulas),
                TotalAusencias = frequenciaAlunoPeriodos.Sum(f => f.TotalAusencias),
                TotalCompensacoes = frequenciaAlunoPeriodos.Sum(f => f.TotalCompensacoes),
            };

            if (aulasComponentesTurmas.Any())
                disciplinasAluno = aulasComponentesTurmas.Where(a => request.CodigosDisciplinasTurma.Any(c => c == a.ComponenteCurricularCodigo))
                                                         .Select(a => a.ComponenteCurricularCodigo).Distinct().ToArray();

            var frequenciaAlunoObtidoIndividual = new List<FrequenciaAluno>();
            foreach (var matricula in request.MatriculasAlunoNaTurma)
            {
                frequenciaAlunoObtidoIndividual
                    .Add(await ObterTotalSomadoIndividualmente(request.CodigosTurmas, request.TipoCalendarioId, request.CodigoAluno, frequenciaAluno, disciplinasAluno, matricula.dataMatricula));
            }

            if (frequenciaAluno.TotalAulas >= frequenciaAlunoObtidoIndividual.Sum(f => f.TotalAulas))
            {
                frequenciaAluno = new FrequenciaAluno()
                {
                    TotalAulas = frequenciaAlunoObtidoIndividual.Sum(f => f.TotalAulas),
                    TotalAusencias = frequenciaAlunoObtidoIndividual.Sum(f => f.TotalAusencias),
                    TotalCompensacoes = frequenciaAlunoObtidoIndividual.Sum(f => f.TotalCompensacoes)
                };
            }

            if (frequenciaAluno?.TotalAulas == 0)
                return string.Empty;
            else if (frequenciaAluno.EhNulo() && (aulasComponentesTurmas.EhNulo() || !aulasComponentesTurmas.Any()))
                return FrequenciaAluno.FormatarPercentual(0);
            else if (frequenciaAluno?.PercentualFrequencia > 0)
                return frequenciaAluno.PercentualFrequenciaFormatado;
            else if (frequenciaAluno?.PercentualFrequencia == 0 && frequenciaAluno?.TotalAulas == frequenciaAluno?.TotalAusencias && frequenciaAluno?.TotalCompensacoes == 0)
                return FrequenciaAluno.FormatarPercentual(0);

            return string.Empty;
        }

        private async Task<FrequenciaAluno> ObterTotalSomadoIndividualmente(string[] turmasCodigo, long tipoCalendarioId, string codigoAluno, FrequenciaAluno frequenciaGeralObtida, string[] disciplinasAluno, DateTime? dataMatriculaTurmaFiltro)
        {
            var periodos = new long[] { };
            var periodosEscolaresTipoCalendario = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

            if (periodosEscolaresTipoCalendario.Any())
                periodos = periodosEscolaresTipoCalendario.Select(p => p.Id).ToArray();

            if (disciplinasAluno.Any())
            {
                var frequenciasDoAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo
                .ObterPorAlunoTurmasDisciplinasDataAsync(codigoAluno, TipoFrequenciaAluno.PorDisciplina, disciplinasAluno, turmasCodigo, new int[] { }, !periodos.Any() ? null : periodos);

                if (dataMatriculaTurmaFiltro.HasValue)
                    frequenciasDoAluno = frequenciasDoAluno.Where(f => f.PeriodoFim > dataMatriculaTurmaFiltro);

                if (frequenciasDoAluno.Any())
                {
                    return new FrequenciaAluno()
                    {
                        TotalAulas = frequenciasDoAluno.Sum(f => f.TotalAulas),
                        TotalAusencias = frequenciasDoAluno.Sum(f => f.TotalAusencias),
                        TotalCompensacoes = frequenciasDoAluno.Sum(f => f.TotalCompensacoes > f.TotalAusencias ? f.TotalAusencias : f.TotalCompensacoes)
                    };
                }

            }
            return frequenciaGeralObtida;
        }
    }
}

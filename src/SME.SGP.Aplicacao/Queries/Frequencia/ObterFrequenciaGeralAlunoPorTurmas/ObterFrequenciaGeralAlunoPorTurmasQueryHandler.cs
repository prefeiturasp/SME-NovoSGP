using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
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

            frequenciaAlunoPeriodos.AddRange(await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaGeralAlunoPorTurmas(request.CodigoAluno, request.CodigosTurmas, request.TipoCalendarioId));
            var bimestres = await mediator.Send(new ObterBimestresPorTipoCalendarioQuery(request.TipoCalendarioId));

            var aulasComponentesTurmas = await mediator.Send(new ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery(request.CodigosTurmas,
                                                                                                                         request.TipoCalendarioId,
                                                                                                                         new string[] { },
                                                                                                                         bimestres.ToArray()));

            int quantidadeTotalAulas = aulasComponentesTurmas.Sum(a => a.AulasQuantidade);

            foreach(var aulaComponenteTurma in aulasComponentesTurmas)
            {
                if (!frequenciaAlunoPeriodos.Any(a => a.TurmaId == aulaComponenteTurma.TurmaCodigo 
                                                   && a.DisciplinaId == aulaComponenteTurma.ComponenteCurricularCodigo 
                                                   && a.Bimestre == aulaComponenteTurma.Bimestre))
                {
                    frequenciaAlunoPeriodos.Add(new FrequenciaAluno()
                    {
                        CodigoAluno = request.CodigoAluno,
                        DisciplinaId = aulaComponenteTurma.ComponenteCurricularCodigo,
                        TurmaId = aulaComponenteTurma.TurmaCodigo,
                        TotalAulas = aulaComponenteTurma.AulasQuantidade,
                        Bimestre = aulaComponenteTurma.Bimestre,
                        PeriodoEscolarId = aulaComponenteTurma.PeriodoEscolarId
                    });
                }
            }

            var frequenciaAluno = new FrequenciaAluno()
            {
                TotalAulas = quantidadeTotalAulas,
                TotalAusencias = frequenciaAlunoPeriodos.Sum(f => f.TotalAusencias),
                TotalCompensacoes = frequenciaAlunoPeriodos.Sum(f => f.TotalCompensacoes),
            };

            if(aulasComponentesTurmas.Any())
                disciplinasAluno = aulasComponentesTurmas.Select(a => a.ComponenteCurricularCodigo).Distinct().ToArray();

            var frequenciaAlunoObtidoIndividual = await ObterTotalSomadoIndividualmente(request.CodigosTurmas, request.TipoCalendarioId, request.CodigoAluno, frequenciaAluno, disciplinasAluno, request.DataMatriculaTurmaFiltro);

            if (frequenciaAluno.TotalAulas >= frequenciaAlunoObtidoIndividual.TotalAulas)
                frequenciaAluno = frequenciaAlunoObtidoIndividual;

            if (frequenciaAluno == null && aulasComponentesTurmas == null || aulasComponentesTurmas.Count() == 0)
                return "";

            else if (frequenciaAluno?.PercentualFrequencia > 0)
                return frequenciaAluno.PercentualFrequencia.ToString();

            else if (frequenciaAluno?.PercentualFrequencia == 0 && frequenciaAluno?.TotalAulas == frequenciaAluno?.TotalAusencias && frequenciaAluno?.TotalCompensacoes == 0)
                return "0";

            else if (aulasComponentesTurmas.Any())
                return "100";

            return "";
        }

        private async Task<FrequenciaAluno> ObterTotalSomadoIndividualmente(string[] turmasCodigo, long tipoCalendarioId, string codigoAluno, FrequenciaAluno frequenciaGeralObtida, string[] disciplinasAluno, DateTime? dataMatriculaTurmaFiltro)
        {
            var periodos = new long[] { };
            var periodosEscolaresTipoCalendario = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
            
            if (periodosEscolaresTipoCalendario.Any())
                periodos = periodosEscolaresTipoCalendario.Select(p => p.Id).ToArray();

            var frequenciasDoAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoTurmasDisciplinasDataAsync(codigoAluno, TipoFrequenciaAluno.PorDisciplina, disciplinasAluno, turmasCodigo, new int[] { }, !periodos.Any()? null : periodos);

            if (dataMatriculaTurmaFiltro.HasValue)
                frequenciasDoAluno = frequenciasDoAluno.Where(f => f.PeriodoFim > dataMatriculaTurmaFiltro);

            if (frequenciasDoAluno.Any())
            {
                return new FrequenciaAluno()
                {
                    TotalAulas = frequenciaGeralObtida.TotalAulas,
                    TotalAusencias = frequenciasDoAluno.Sum(f => f.TotalAusencias),
                    TotalCompensacoes = frequenciasDoAluno.Sum(f => f.TotalCompensacoes)
                };
            }

            return frequenciaGeralObtida;
        }        
    }
}

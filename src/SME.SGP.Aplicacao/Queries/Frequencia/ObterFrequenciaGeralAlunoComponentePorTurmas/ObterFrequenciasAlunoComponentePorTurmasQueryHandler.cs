using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasAlunoComponentePorTurmasQueryHandler : IRequestHandler<ObterFrequenciasAlunoComponentePorTurmasQuery, IEnumerable<FrequenciaAluno>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IMediator mediator;

        public ObterFrequenciasAlunoComponentePorTurmasQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo,
                                                                        IMediator mediator)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<FrequenciaAluno>> Handle(ObterFrequenciasAlunoComponentePorTurmasQuery request, CancellationToken cancellationToken)
        {
            var frequenciaAlunoPeriodos = new List<FrequenciaAluno>();
            frequenciaAlunoPeriodos.AddRange(await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaComponentesAlunoPorTurmas(request.CodigoAluno, request.CodigosTurmas, request.TipoCalendarioId, request.Bimestre));
           
            var bimestres = request.Bimestre > 0 ?
                new int[] { request.Bimestre } :
                await mediator.Send(new ObterBimestresPorTipoCalendarioQuery(request.TipoCalendarioId));

            var aulasComponentesTurmas = await mediator.Send(new ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery(request.CodigosTurmas,
                                                                                                                         request.TipoCalendarioId,
                                                                                                                         new string[] { },
                                                                                                                         bimestres.ToArray()));
            foreach (var aulaComponenteTurma in aulasComponentesTurmas)
            {
                var codigosComponentesConsiderados = new List<string>() { aulaComponenteTurma.ComponenteCurricularCodigo };
                if (!frequenciaAlunoPeriodos.Any(a => a.TurmaId == aulaComponenteTurma.TurmaCodigo
                                                   && codigosComponentesConsiderados.Contains(a.DisciplinaId)
                                                   && a.Bimestre == aulaComponenteTurma.Bimestre))
                {
                    frequenciaAlunoPeriodos.Add(new FrequenciaAluno()
                    {
                        CodigoAluno = request.CodigoAluno,
                        DisciplinaId = aulaComponenteTurma.ComponenteCurricularCodigo,
                        TurmaId = aulaComponenteTurma.TurmaCodigo,
                        TotalAulas = 0,
                        Bimestre = aulaComponenteTurma.Bimestre,
                        PeriodoEscolarId = aulaComponenteTurma.PeriodoEscolarId,
                        TotalPresencas = 0,
                        PeriodoInicio = aulaComponenteTurma.PeriodoInicio,
                        PeriodoFim = aulaComponenteTurma.PeriodoFim,
                        Professor = aulaComponenteTurma.Professor
                    });
                }
            }
            
            if (request.DataMatricula.HasValue)
                frequenciaAlunoPeriodos = frequenciaAlunoPeriodos.Where(f => f.PeriodoFim > request.DataMatricula.Value).ToList();

            return frequenciaAlunoPeriodos
                .GroupBy(a => (a.DisciplinaId, a.Professor))
                .Select(f => new FrequenciaAluno()
                {
                    DisciplinaId = f.Key.DisciplinaId,
                    TotalAulas = f.Sum(x => x.TotalAulas),
                    TotalAusencias = f.Sum(x => x.TotalAusencias),
                    TotalCompensacoes = f.Sum(x => x.TotalCompensacoes),
                    Professor = f.FirstOrDefault()?.Professor
                });
        }
    }
}

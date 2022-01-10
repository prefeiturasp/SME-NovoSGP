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
                TotalAulas = frequenciaAlunoPeriodos.Sum(f => f.TotalAulas),
                TotalAusencias = frequenciaAlunoPeriodos.Sum(f => f.TotalAusencias),
                TotalCompensacoes = frequenciaAlunoPeriodos.Sum(f => f.TotalCompensacoes),
            };

            return frequenciaAlunoPeriodos
                .GroupBy(a => a.DisciplinaId)
                .Select(f => new FrequenciaAluno()
                {
                    DisciplinaId = f.Key,
                    TotalAulas = f.Sum(x => x.TotalAulas),
                    TotalAusencias = f.Sum(x => x.TotalAusencias),
                    TotalCompensacoes = f.Sum(x => x.TotalCompensacoes),
                });
        }
    }
}

using MediatR;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentosPorTurmaPeriodoCCQuery : IRequest<IEnumerable<FechamentoPorTurmaPeriodoCCDto>>
    {
        public ObterFechamentosPorTurmaPeriodoCCQuery(long periodoEscolarId, long turmaId, long componenteCurricularId)
        {
            PeriodoEscolarId = periodoEscolarId;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long PeriodoEscolarId { get; }
        public long TurmaId { get; }
        public long ComponenteCurricularId { get; }
    }
}

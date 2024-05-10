using MediatR;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentosPorTurmaPeriodoCCQuery : IRequest<IEnumerable<FechamentoPorTurmaPeriodoCCDto>>
    {
        public ObterFechamentosPorTurmaPeriodoCCQuery(long periodoEscolarId, long turmaId, long componenteCurricularId, bool ehRegencia = false)
        {
            PeriodoEscolarId = periodoEscolarId;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            EhRegencia = ehRegencia;
        }

        public long PeriodoEscolarId { get; }
        public long TurmaId { get; }
        public long ComponenteCurricularId { get; }
        public bool EhRegencia { get; set; }
    }
}

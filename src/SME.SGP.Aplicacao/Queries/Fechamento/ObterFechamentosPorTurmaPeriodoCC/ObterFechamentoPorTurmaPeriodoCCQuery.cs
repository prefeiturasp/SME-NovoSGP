using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentosPorTurmaPeriodoCCQuery : IRequest<IEnumerable<FechamentoTurmaDisciplina>>
    {
        public ObterFechamentosPorTurmaPeriodoCCQuery(long periodoEscolarId, long turmaId, long componenteCurricularId)
        {
            PeriodoEscolarId = periodoEscolarId;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long PeriodoEscolarId { get; internal set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }

    }
}

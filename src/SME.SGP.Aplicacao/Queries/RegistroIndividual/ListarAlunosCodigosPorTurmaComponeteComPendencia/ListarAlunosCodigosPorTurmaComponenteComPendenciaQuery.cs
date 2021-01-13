using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ListarAlunosCodigosPorTurmaComponenteComPendenciaQuery : IRequest<IEnumerable<long>>
    {
        public ListarAlunosCodigosPorTurmaComponenteComPendenciaQuery(long turmaId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }
}

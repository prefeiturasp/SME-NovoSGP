using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaPreDefinidaPorTurmaComponenteQuery : IRequest<IEnumerable<FrequenciaPreDefinidaDto>>
    {
        public ObterFrequenciaPreDefinidaPorTurmaComponenteQuery(long turmaId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; }
        public long ComponenteCurricularId { get; }
    }
}

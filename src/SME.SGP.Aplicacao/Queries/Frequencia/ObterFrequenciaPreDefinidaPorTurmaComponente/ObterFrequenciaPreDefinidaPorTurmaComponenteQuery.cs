using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaPreDefinidaPorTurmaComponenteQuery : IRequest<IEnumerable<FrequenciaPreDefinidaDto>>
    {
        public ObterFrequenciaPreDefinidaPorTurmaComponenteQuery(string turmaCodigo, long componenteCurricularId)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularId = componenteCurricularId;
        }

        public string TurmaCodigo { get; }
        public long ComponenteCurricularId { get; }
    }
}

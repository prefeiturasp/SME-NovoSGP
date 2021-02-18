using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosCodigoPorItineranciasQuery : IRequest<IEnumerable<ItineranciaCodigoAlunoDto>>
    {
        public ObterAlunosCodigoPorItineranciasQuery(long[] itineranciasIds)
        {
            ItineranciasIds = itineranciasIds;
        }
        public long[] ItineranciasIds { get; set; }
    }
}

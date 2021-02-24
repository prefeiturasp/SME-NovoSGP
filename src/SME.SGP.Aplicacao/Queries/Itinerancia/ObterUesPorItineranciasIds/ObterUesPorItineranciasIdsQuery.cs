using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUesPorItineranciasIdsQuery : IRequest<IEnumerable<ItineranciaIdUeInfosDto>>
    {
        public ObterUesPorItineranciasIdsQuery(long[] itineranciaIds)
        {
            ItineranciaIds = itineranciaIds;
        }

        public long[] ItineranciaIds { get; set; }
    }
}

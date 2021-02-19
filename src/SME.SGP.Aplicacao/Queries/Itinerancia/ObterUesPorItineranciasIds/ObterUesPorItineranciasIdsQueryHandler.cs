using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUesPorItineranciasIdsQueryHandler : IRequestHandler<ObterUesPorItineranciasIdsQuery, IEnumerable<ItineranciaIdUeInfosDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterUesPorItineranciasIdsQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }
        public async Task<IEnumerable<ItineranciaIdUeInfosDto>> Handle(ObterUesPorItineranciasIdsQuery request, CancellationToken cancellationToken)
        {
            return await repositorioItinerancia.ObterUesItineranciaPorIds(request.ItineranciaIds);
        }
    }
}

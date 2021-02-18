using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciaPorIdQueryHandler : IRequestHandler<ObterItineranciaPorIdQuery, Itinerancia>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterItineranciaPorIdQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<Itinerancia> Handle(ObterItineranciaPorIdQuery request, CancellationToken cancellationToken)
                       => await repositorioItinerancia.ObterEntidadeCompleta(request.Id);
    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosItineranciaPorIdQueryHandler : IRequestHandler<ObterObjetivosItineranciaPorIdQuery, IEnumerable<ItineranciaObjetivoDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterObjetivosItineranciaPorIdQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<IEnumerable<ItineranciaObjetivoDto>> Handle(ObterObjetivosItineranciaPorIdQuery request, CancellationToken cancellationToken)
                => await repositorioItinerancia.ObterObjetivosItineranciaPorId(request.Id);
    }
}

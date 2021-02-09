using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUesItineranciaPorIdQueryHandler : IRequestHandler<ObterUesItineranciaPorIdQuery, IEnumerable<ItineranciaUeDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterUesItineranciaPorIdQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<IEnumerable<ItineranciaUeDto>> Handle(ObterUesItineranciaPorIdQuery request, CancellationToken cancellationToken)
                       => await repositorioItinerancia.ObterUesItineranciaPorId(request.Id);
    }
}

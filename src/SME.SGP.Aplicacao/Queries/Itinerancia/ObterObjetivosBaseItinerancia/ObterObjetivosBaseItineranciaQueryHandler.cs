using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosBaseItineranciaQueryHandler : IRequestHandler<ObterObjetivosBaseItineranciaQuery, IEnumerable<ItineranciaObjetivosBaseDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterObjetivosBaseItineranciaQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<IEnumerable<ItineranciaObjetivosBaseDto>> Handle(ObterObjetivosBaseItineranciaQuery request, CancellationToken cancellationToken)
                => await repositorioItinerancia.ObterObjetivosBase();
    }
}

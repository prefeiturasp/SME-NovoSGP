using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosItineranciaQueryHandler : IRequestHandler<ObterAnosLetivosItineranciaQuery, IEnumerable<long>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterAnosLetivosItineranciaQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<IEnumerable<long>> Handle(ObterAnosLetivosItineranciaQuery request, CancellationToken cancellationToken)
                => await repositorioItinerancia.ObterAnosLetivosItinerancia();
    }
}

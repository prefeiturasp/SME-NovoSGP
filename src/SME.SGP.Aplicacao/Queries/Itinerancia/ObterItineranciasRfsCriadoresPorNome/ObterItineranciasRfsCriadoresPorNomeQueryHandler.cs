using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciasRfsCriadoresPorNomeQueryHandler : IRequestHandler<ObterItineranciasRfsCriadoresPorNomeQuery, IEnumerable<ItineranciaNomeRfCriadorRetornoDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterItineranciasRfsCriadoresPorNomeQueryHandler(IRepositorioItinerancia repositorioItinerancia )
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new System.ArgumentNullException(nameof(repositorioItinerancia));
        }
        public async Task<IEnumerable<ItineranciaNomeRfCriadorRetornoDto>> Handle(ObterItineranciasRfsCriadoresPorNomeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioItinerancia.ObterRfsCriadoresPorNome(request.NomeParaBusca);
        }
    }
}

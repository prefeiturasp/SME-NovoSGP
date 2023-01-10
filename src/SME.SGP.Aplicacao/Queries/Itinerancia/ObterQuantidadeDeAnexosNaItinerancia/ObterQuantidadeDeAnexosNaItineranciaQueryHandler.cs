using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeDeAnexosNaItineranciaQueryHandler: IRequestHandler<ObterQuantidadeDeAnexosNaItineranciaQuery,int>

    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterQuantidadeDeAnexosNaItineranciaQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<int> Handle(ObterQuantidadeDeAnexosNaItineranciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioItinerancia.ObterQuantidadeDeAnexosNaItinerancia(request.ItineranciaId);
        }
    }
}
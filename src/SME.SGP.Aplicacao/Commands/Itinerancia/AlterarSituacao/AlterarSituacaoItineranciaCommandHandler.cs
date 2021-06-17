using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarSituacaoItineranciaCommandHandler : IRequestHandler<AlterarSituacaoItineranciaCommand, bool>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public AlterarSituacaoItineranciaCommandHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<bool> Handle(AlterarSituacaoItineranciaCommand request, CancellationToken cancellationToken)
            => (await repositorioItinerancia.AtualizarStatusItinerancia(request.ItineranciaId, (int)request.Situacao)) > 0;
    }
}

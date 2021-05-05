using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaUeCommandHandler : IRequestHandler<ExcluirItineranciaUeCommand, bool>
    {
        private readonly IRepositorioItineranciaUe repositorioItineranciaUe;

        public ExcluirItineranciaUeCommandHandler(IRepositorioItineranciaUe repositorioItineranciaUe)
        {
            this.repositorioItineranciaUe = repositorioItineranciaUe ?? throw new ArgumentNullException(nameof(repositorioItineranciaUe));
        }

        public async Task<bool> Handle(ExcluirItineranciaUeCommand request, CancellationToken cancellationToken)
        {
            await repositorioItineranciaUe.ExcluirItineranciaUe(request.UeId, request.ItineranciaId);
            return true;
        }
    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverEventosItineranciaCommandHandler : IRequestHandler<RemoverEventosItineranciaCommand, bool>
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioItineranciaEvento repositorioItineranciaEvento;

        public RemoverEventosItineranciaCommandHandler(IRepositorioEvento repositorioEvento, IRepositorioItineranciaEvento repositorioItineranciaEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioItineranciaEvento = repositorioItineranciaEvento ?? throw new ArgumentNullException(nameof(repositorioItineranciaEvento));
        }

        public async Task<bool> Handle(RemoverEventosItineranciaCommand request, CancellationToken cancellationToken)
        {
            var eventosIds = await repositorioItineranciaEvento.ObterEventosIdsPorItinerancia(request.ItineranciaId);
            foreach (var eventoId in eventosIds)
                await repositorioEvento.RemoverLogico(eventoId);

            await repositorioItineranciaEvento.RemoverLogico(request.ItineranciaId, "itinerancia_id");

            return true;
        }
    }
}

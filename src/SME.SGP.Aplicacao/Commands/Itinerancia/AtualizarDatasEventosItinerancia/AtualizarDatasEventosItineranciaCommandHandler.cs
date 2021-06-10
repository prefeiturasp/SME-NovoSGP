using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarDatasEventosItineranciaCommandHandler : IRequestHandler<AtualizarDatasEventosItineranciaCommand, bool>
    {
        private readonly IRepositorioItineranciaEvento repositorioItineranciaEvento;
        private readonly IRepositorioEvento repositorioEvento;

        public AtualizarDatasEventosItineranciaCommandHandler(IRepositorioItineranciaEvento repositorioItineranciaEvento, IRepositorioEvento repositorioEvento)
        {
            this.repositorioItineranciaEvento = repositorioItineranciaEvento ?? throw new ArgumentNullException(nameof(repositorioItineranciaEvento));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<bool> Handle(AtualizarDatasEventosItineranciaCommand request, CancellationToken cancellationToken)
        {
            var eventos = await repositorioItineranciaEvento.ObterEventosPorItineranciaId(request.ItineranciaId);
            foreach(var evento in eventos)
            {
                evento.DataInicio = request.DataEvento;
                evento.DataFim = request.DataEvento;

                await repositorioEvento.SalvarAsync(evento);
            }

            return true;
        }
    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorEventoQueryHandler : IRequestHandler<ObterTipoCalendarioIdPorEventoQuery, long>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterTipoCalendarioIdPorEventoQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<long> Handle(ObterTipoCalendarioIdPorEventoQuery request, CancellationToken cancellationToken)
            => await repositorioEvento.ObterTipoCalendarioIdPorEvento(request.EventoId);
    }
}

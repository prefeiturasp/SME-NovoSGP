using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosPorTipoCalendarioQueryHandler : IRequestHandler<ObterEventosPorTipoCalendarioQuery, IEnumerable<Evento>>
    {
        private readonly IRepositorioEvento repositorioEvento;
        public ObterEventosPorTipoCalendarioQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public Task<IEnumerable<Evento>> Handle(ObterEventosPorTipoCalendarioQuery request, CancellationToken cancellationToken)
                => repositorioEvento.ObterEventosPorTipoDeCalendarioAsync(request.TipoCalendarioId);
    }
}

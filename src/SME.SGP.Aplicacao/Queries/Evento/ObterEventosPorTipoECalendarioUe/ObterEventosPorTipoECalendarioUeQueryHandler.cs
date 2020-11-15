using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosPorTipoECalendarioUeQueryHandler : IRequestHandler<ObterEventosPorTipoECalendarioUeQuery, IEnumerable<Evento>>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterEventosPorTipoECalendarioUeQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<IEnumerable<Evento>> Handle(ObterEventosPorTipoECalendarioUeQuery request, CancellationToken cancellationToken)
            => await repositorioEvento.ObterEventosPorTipoECalendarioUe(request.TipoCalendarioId, request.UeCodigo, request.TipoEvento);
    }
}

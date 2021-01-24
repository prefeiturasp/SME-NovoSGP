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
    public class ObterEventosPorTipoDeCalendarioDreUeQueryHandler : IRequestHandler<ObterEventosPorTipoDeCalendarioDreUeQuery, IEnumerable<Evento>>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterEventosPorTipoDeCalendarioDreUeQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public Task<IEnumerable<Evento>> Handle(ObterEventosPorTipoDeCalendarioDreUeQuery request, CancellationToken cancellationToken)
            => Task.FromResult(repositorioEvento.ObterEventosPorTipoDeCalendarioDreUe(request.TipoCalendarioId, request.DreCodigo, request.UeCodigo, request.EhEventoSME, request.FiltroDreUe));
    }
}

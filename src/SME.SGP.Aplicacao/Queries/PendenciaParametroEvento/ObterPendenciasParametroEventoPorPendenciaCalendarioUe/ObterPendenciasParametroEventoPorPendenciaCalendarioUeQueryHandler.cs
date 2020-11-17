using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParametroEventoPorPendenciaCalendarioUeQueryHandler : IRequestHandler<ObterPendenciasParametroEventoPorPendenciaCalendarioUeQuery, IEnumerable<PendenciaParametroEvento>>
    {
        private readonly IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento;

        public ObterPendenciasParametroEventoPorPendenciaCalendarioUeQueryHandler(IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento)
        {
            this.repositorioPendenciaParametroEvento = repositorioPendenciaParametroEvento ?? throw new ArgumentNullException(nameof(repositorioPendenciaParametroEvento));
        }

        public async Task<IEnumerable<PendenciaParametroEvento>> Handle(ObterPendenciasParametroEventoPorPendenciaCalendarioUeQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaParametroEvento.ObterPendenciasEventoPorPendenciaCalendarioUe(request.PendenciaCalendarioUeId);
    }
}

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
    public class ObterPendenciaParametroEventoPorCalendarioUeParametroQueryHandler : IRequestHandler<ObterPendenciaParametroEventoPorCalendarioUeParametroQuery, PendenciaParametroEvento>
    {
        private readonly IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento;

        public ObterPendenciaParametroEventoPorCalendarioUeParametroQueryHandler(IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento)
        {
            this.repositorioPendenciaParametroEvento = repositorioPendenciaParametroEvento ?? throw new ArgumentNullException(nameof(repositorioPendenciaParametroEvento));
        }

        public async Task<PendenciaParametroEvento> Handle(ObterPendenciaParametroEventoPorCalendarioUeParametroQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaParametroEvento.ObterPendenciaEventoPorCalendarioUeParametro(request.TipoCalendarioId, request.UeId, request.ParametroSistemaId);
    }
}

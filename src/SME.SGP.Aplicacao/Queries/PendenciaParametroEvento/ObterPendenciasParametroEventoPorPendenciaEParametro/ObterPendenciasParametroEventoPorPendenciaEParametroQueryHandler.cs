using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParametroEventoPorPendenciaEParametroQueryHandler : IRequestHandler<ObterPendenciasParametroEventoPorPendenciaEParametroQuery, PendenciaParametroEvento>
    {
        private readonly IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento;

        public ObterPendenciasParametroEventoPorPendenciaEParametroQueryHandler(IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento)
        {
            this.repositorioPendenciaParametroEvento = repositorioPendenciaParametroEvento ?? throw new ArgumentNullException(nameof(repositorioPendenciaParametroEvento));
        }

        public async Task<PendenciaParametroEvento> Handle(ObterPendenciasParametroEventoPorPendenciaEParametroQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaParametroEvento.ObterPendenciaEventoPorPendenciaEParametroId(request.PendenciaId, request.ParametroId);
    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParametroEventoPorPendenciaQueryHandler : IRequestHandler<ObterPendenciasParametroEventoPorPendenciaQuery, IEnumerable<PendenciaParametroEventoDto>>
    {
        private readonly IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento;

        public ObterPendenciasParametroEventoPorPendenciaQueryHandler(IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento)
        {
            this.repositorioPendenciaParametroEvento = repositorioPendenciaParametroEvento ?? throw new ArgumentNullException(nameof(repositorioPendenciaParametroEvento));
        }

        public async Task<IEnumerable<PendenciaParametroEventoDto>> Handle(ObterPendenciasParametroEventoPorPendenciaQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaParametroEvento.ObterPendenciasEventoPorPendenciaId(request.PendenciaId);
    }
}

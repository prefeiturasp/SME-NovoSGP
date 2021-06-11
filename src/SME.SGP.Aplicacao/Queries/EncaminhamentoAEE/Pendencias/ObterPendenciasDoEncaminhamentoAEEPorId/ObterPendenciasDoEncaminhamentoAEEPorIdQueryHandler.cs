using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasDoEncaminhamentoAEEPorIdQueryHandler : IRequestHandler<ObterPendenciasDoEncaminhamentoAEEPorIdQuery, IEnumerable<PendenciaEncaminhamentoAEE>>
    {
        private readonly IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE;

        public ObterPendenciasDoEncaminhamentoAEEPorIdQueryHandler(IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE)
        {
            this.repositorioPendenciaEncaminhamentoAEE = repositorioPendenciaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaEncaminhamentoAEE));
        }

        public async Task<IEnumerable<PendenciaEncaminhamentoAEE>> Handle(ObterPendenciasDoEncaminhamentoAEEPorIdQuery request, CancellationToken cancellationToken)        
            => await repositorioPendenciaEncaminhamentoAEE.ObterPendenciasPorEncaminhamentoAEEId(request.EncaminhamentoAEEId);

    }
}

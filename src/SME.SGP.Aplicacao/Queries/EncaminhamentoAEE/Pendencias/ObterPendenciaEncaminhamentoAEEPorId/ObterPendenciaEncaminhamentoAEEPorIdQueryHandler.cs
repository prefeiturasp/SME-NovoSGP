using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaEncaminhamentoAEEPorIdQueryHandler : IRequestHandler<ObterPendenciaEncaminhamentoAEEPorIdQuery, PendenciaEncaminhamentoAEE>
    {
        private readonly IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE;

        public ObterPendenciaEncaminhamentoAEEPorIdQueryHandler(IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE)
        {
            this.repositorioPendenciaEncaminhamentoAEE = repositorioPendenciaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaEncaminhamentoAEE));
        }


        public async Task<PendenciaEncaminhamentoAEE> Handle(ObterPendenciaEncaminhamentoAEEPorIdQuery request, CancellationToken cancellationToken)        
            => await repositorioPendenciaEncaminhamentoAEE.ObterPorEncaminhamentoAEEId(request.EncaminhamentoAEEId);

    }
}

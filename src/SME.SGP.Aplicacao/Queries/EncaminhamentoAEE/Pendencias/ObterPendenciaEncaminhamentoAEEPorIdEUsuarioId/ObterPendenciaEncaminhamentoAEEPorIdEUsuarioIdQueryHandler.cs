using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQueryHandler : IRequestHandler<ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery, PendenciaEncaminhamentoAEE>
    {
        private readonly IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE;

        public ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQueryHandler(IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE)
        {
            this.repositorioPendenciaEncaminhamentoAEE = repositorioPendenciaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaEncaminhamentoAEE));
        }

        public async Task<PendenciaEncaminhamentoAEE> Handle(ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery request, CancellationToken cancellationToken)        
            => await repositorioPendenciaEncaminhamentoAEE.ObterPorEncaminhamentoAEEIdEUsuarioId(request.EncaminhamentoAEEId);

    }
}

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
    public class SalvarPendenciaEncaminhamentoAEECommandHandler : IRequestHandler<SalvarPendenciaEncaminhamentoAEECommand, long>
    {
        private readonly IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE;

        public SalvarPendenciaEncaminhamentoAEECommandHandler(IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE)
        {
            this.repositorioPendenciaEncaminhamentoAEE = repositorioPendenciaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaEncaminhamentoAEE));
        }

        public async Task<long> Handle(SalvarPendenciaEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            PendenciaEncaminhamentoAEE pendenciaEncaminhamentoAEE = MapearParaEnticadeEncaminhamentoAEE(request);

            return await repositorioPendenciaEncaminhamentoAEE.SalvarAsync(pendenciaEncaminhamentoAEE);
        }

        private static PendenciaEncaminhamentoAEE MapearParaEnticadeEncaminhamentoAEE(SalvarPendenciaEncaminhamentoAEECommand request)
        {
            return new PendenciaEncaminhamentoAEE
            {
                EncaminhamentoAEEId = request.EncaminhamentoAEEId,
                PendenciaId = request.PendenciaId
            };
        }
    }
}

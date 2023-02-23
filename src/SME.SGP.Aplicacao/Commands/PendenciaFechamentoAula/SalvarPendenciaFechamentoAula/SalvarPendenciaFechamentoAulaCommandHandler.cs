using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaFechamentoAulaCommandHandler : IRequestHandler<SalvarPendenciaFechamentoAulaCommand, bool>
    {
        private readonly IRepositorioPendenciaFechamentoAula repositorioPendenciaFechamentoAula;

        public SalvarPendenciaFechamentoAulaCommandHandler(IRepositorioPendenciaFechamentoAula repositorioPendenciaFechamentoAula)
        {
            this.repositorioPendenciaFechamentoAula = repositorioPendenciaFechamentoAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamentoAula));
        }

        public async Task<bool> Handle(SalvarPendenciaFechamentoAulaCommand request, CancellationToken cancellationToken)
        {
            await repositorioPendenciaFechamentoAula.SalvarAsync(new PendenciaFechamentoAula()
            {
                AulaId = request.AulaId,
                PendenciaFechamentoId = request.PendenciaFechamentoId
            });

            return true;
        }
    }
}

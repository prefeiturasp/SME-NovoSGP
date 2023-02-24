using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaFechamentoAtividadeAvaliativaCommandHandler : IRequestHandler<SalvarPendenciaFechamentoAtividadeAvaliativaCommand, bool>
    {
        private readonly IRepositorioPendenciaFechamentoAtividadeAvaliativa repositorioPendenciaFechamentoAtividadeAvaliativa;

        public SalvarPendenciaFechamentoAtividadeAvaliativaCommandHandler(IRepositorioPendenciaFechamentoAtividadeAvaliativa repositorioPendenciaFechamentoAtividadeAvaliativa)
        {
            this.repositorioPendenciaFechamentoAtividadeAvaliativa = repositorioPendenciaFechamentoAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamentoAtividadeAvaliativa));
        }

        public async Task<bool> Handle(SalvarPendenciaFechamentoAtividadeAvaliativaCommand request, CancellationToken cancellationToken)
        {
            await repositorioPendenciaFechamentoAtividadeAvaliativa.SalvarAsync(new PendenciaFechamentoAtividadeAvaliativa()
            {
                AtividadeAvaliativaId = request.AtividadeAvaliativaId,
                PendenciaFechamentoId = request.PendenciaFechamentoId
            });

            return true;
        }
    }
}

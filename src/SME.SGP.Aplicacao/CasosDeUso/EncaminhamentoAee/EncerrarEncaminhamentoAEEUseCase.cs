using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EncerrarEncaminhamentoAEEUseCase : IEncerrarEncaminhamentoAEEUseCase
    {
        private readonly IMediator mediator;

        public EncerrarEncaminhamentoAEEUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long encaminhamentoId, string motivoEncerramento)
        {
            await mediator.Send(new EncerrarEncaminhamentoAEECommand(encaminhamentoId, motivoEncerramento));

            await ExcluirPendenciasEncaminhamentoAEE(encaminhamentoId);

            return true;
        }

        private async Task ExcluirPendenciasEncaminhamentoAEE(long encaminhamentoId)
        {
            var pendenciasEncaminhamentoAEE = await mediator.Send(new ObterPendenciasDoEncaminhamentoAEEPorIdQuery(encaminhamentoId));
            if (pendenciasEncaminhamentoAEE != null || !pendenciasEncaminhamentoAEE.Any())
            {
                foreach (var pendenciaEncaminhamentoAEE in pendenciasEncaminhamentoAEE)
                {
                    await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendenciaEncaminhamentoAEE.PendenciaId));
                }
            }
        }
    }
}

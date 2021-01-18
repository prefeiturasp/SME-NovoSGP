using MediatR;
using System;
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
                => await mediator.Send(new EncerrarEncaminhamentoAEECommand(encaminhamentoId, motivoEncerramento));
    }
}

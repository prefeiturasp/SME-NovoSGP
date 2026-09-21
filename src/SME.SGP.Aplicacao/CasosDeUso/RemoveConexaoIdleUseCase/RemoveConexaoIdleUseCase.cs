using MediatR;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoveConexaoIdleUseCase : IRemoveConexaoIdleUseCase
    {
        private readonly IMediator mediator;

        public RemoveConexaoIdleUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task Executar()
        {
            await mediator.Send(new RemoveConexaoIdleCommand());
        }
    }
}

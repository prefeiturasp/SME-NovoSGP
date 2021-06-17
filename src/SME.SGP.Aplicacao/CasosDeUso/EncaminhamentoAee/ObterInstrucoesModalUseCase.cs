using MediatR;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInstrucoesModalUseCase : IObterInstrucoesModalUseCase
    {
        private readonly IMediator mediator;

        public ObterInstrucoesModalUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Executar()
                => await mediator.Send(new ObterInstrucoesModalQuery());
    }
}

using MediatR;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarParaAnaliseEncaminhamentoAEEUseCase : IEnviarParaAnaliseEncaminhamentoAEEUseCase
    {
        private readonly IMediator mediator;

        public EnviarParaAnaliseEncaminhamentoAEEUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long encaminhamentoId)
                => await mediator.Send(new EnviarParaAnaliseEncaminhamentoAEECommand(encaminhamentoId));
    }
}

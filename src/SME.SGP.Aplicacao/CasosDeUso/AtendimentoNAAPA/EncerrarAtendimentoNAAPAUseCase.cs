using MediatR;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EncerrarAtendimentoNAAPAUseCase : IEncerrarAtendimentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public EncerrarAtendimentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long encaminhamentoId, string motivoEncerramento)
        {
            await mediator.Send(new EncerrarAtendimentoNAAPACommand(encaminhamentoId, motivoEncerramento));

            return true;
        }
    }
}

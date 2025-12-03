using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EncerrarEncaminhamentoNAAPAUseCase : IEncerrarAtendimentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public EncerrarEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long encaminhamentoId, string motivoEncerramento)
        {
            await mediator.Send(new EncerrarEncaminhamentoNAAPACommand(encaminhamentoId, motivoEncerramento));

            return true;
        }
    }
}

using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoPeriodoFechamentoReaberturaIniciandoUseCase : INotificacaoPeriodoFechamentoReaberturaIniciando
    {
        private readonly IMediator mediator;

        public NotificacaoPeriodoFechamentoReaberturaIniciandoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            await mediator.Send(mensagemRabbit.ObterObjetoMensagem<ExecutaNotificacaoPeriodoFechamentoIniciandoCommand>());
            return false;
        }
    }
}

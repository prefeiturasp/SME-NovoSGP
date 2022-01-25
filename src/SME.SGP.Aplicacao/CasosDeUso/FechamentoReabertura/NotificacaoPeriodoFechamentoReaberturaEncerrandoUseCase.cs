using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoPeriodoFechamentoReaberturaEncerrandoUseCase : INotificacaoPeriodoFechamentoReaberturaEncerrando
    {
        private readonly IMediator mediator;

        public NotificacaoPeriodoFechamentoReaberturaEncerrandoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            await mediator.Send(mensagemRabbit.ObterObjetoMensagem<ExecutaNotificacaoPeriodoFechamentoEncerrandoCommand>());
            return true;
        }
    }
}

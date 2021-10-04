using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoEncerramentoPlanoAEEUseCase : AbstractUseCase, IEnviarNotificacaoEncerramentoPlanoAEEUseCase
    {
        public EnviarNotificacaoEncerramentoPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var command = mensagemRabbit.ObterObjetoMensagem<NotificarEncerramentoPlanoAEECommand>();

            return await mediator.Send(command);
        }
    }
}

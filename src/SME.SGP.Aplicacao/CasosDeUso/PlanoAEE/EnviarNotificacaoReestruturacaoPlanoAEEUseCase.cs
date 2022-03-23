using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoReestruturacaoPlanoAEEUseCase : AbstractUseCase, IEnviarNotificacaoReestruturacaoPlanoAEEUseCase
    {
        public EnviarNotificacaoReestruturacaoPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var command = mensagemRabbit.ObterObjetoMensagem<EnviarNotificacaoReestruturacaoPlanoAEECommand>();

            return await mediator.Send(command);
        }
    }
}

using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoCriacaoPlanoAEEUseCase : AbstractUseCase, IEnviarNotificacaoCriacaoPlanoAEEUseCase
    {
        public EnviarNotificacaoCriacaoPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem EnviarNotificacaoCriacaoPlanoAEEUseCase", "Rabbit - EnviarNotificacaoCriacaoPlanoAEEUseCase");

            var command = mensagemRabbit.ObterObjetoMensagem<EnviarNotificacaoCriacaoPlanoAEECommand>();

            return await mediator.Send(command);
        }
    }
}

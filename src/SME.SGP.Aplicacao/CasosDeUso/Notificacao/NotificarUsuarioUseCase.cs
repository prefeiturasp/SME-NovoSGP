using MediatR;
using Sentry;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarUsuarioUseCase : AbstractUseCase, INotificarUsuarioUseCase
    {
        public NotificarUsuarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem NotificarUsuarioUseCase", "Rabbit - NotificarUsuarioUseCase");

            var command = mensagemRabbit.ObterObjetoMensagem<NotificarUsuarioCommand>();

            return await mediator.Send(command);
        }
    }
}

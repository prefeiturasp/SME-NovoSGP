using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarUsuarioUseCase : AbstractUseCase, INotificarUsuarioUseCase
    {
        public NotificarUsuarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var command = param.ObterObjetoMensagem<NotificarUsuarioCommand>();

            await mediator.Send(command);

            return true;
        }
    }
}

using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarFilaNotificacaoCriacaoPlanoAEECommandHandler : IRequestHandler<EnviarFilaNotificacaoCriacaoPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;

        public EnviarFilaNotificacaoCriacaoPlanoAEECommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(EnviarFilaNotificacaoCriacaoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            var command = new EnviarNotificacaoCriacaoPlanoAEECommand(request.PlanoAEEId, usuario);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.NotificarCriacaoPlanoAEE, command, Guid.NewGuid(), usuario));

            return true;
        }
    }
}

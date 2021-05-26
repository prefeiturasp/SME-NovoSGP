using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarFilaNotificacaoEncerramentoPlanoAEECommandHandler : IRequestHandler<EnviarFilaNotificacaoEncerramentoPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;

        public EnviarFilaNotificacaoEncerramentoPlanoAEECommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(EnviarFilaNotificacaoEncerramentoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var command = new NotificarEncerramentoPlanoAEECommand(request.PlanoAEEId, usuario);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.NotificarPlanoAEEEncerrado, command, Guid.NewGuid(), usuario));
            SentrySdk.AddBreadcrumb($"Envio Fila de Notificação de Encerramento de PlanoAEE", "RabbitMQ");

            return true;
        }
    }
}

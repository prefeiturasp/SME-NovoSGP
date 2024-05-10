using MediatR;
using SME.SGP.Infra;
using SME.SGP.Notificacoes.Hub.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarExclusaoNotificacaoCommandHandler : AsyncRequestHandler<NotificarExclusaoNotificacaoCommand>
    {
        private readonly IMediator mediator;

        public NotificarExclusaoNotificacaoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(NotificarExclusaoNotificacaoCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNotificacoes.Exclusao,
                                                           new MensagemExclusaoNotificacaoDto(
                                                               request.Codigo,
                                                               (int)request.Status,
                                                               request.UsuarioRf,
                                                               request.AnoAnterior)));
        }
    }
}

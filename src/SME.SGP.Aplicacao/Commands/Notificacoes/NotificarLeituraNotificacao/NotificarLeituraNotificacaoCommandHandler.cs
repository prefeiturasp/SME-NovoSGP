using MediatR;
using SME.SGP.Infra;
using SME.SGP.Notificacoes.Hub.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarLeituraNotificacaoCommandHandler : AsyncRequestHandler<NotificarLeituraNotificacaoCommand>
    {
        private readonly IMediator mediator;

        public NotificarLeituraNotificacaoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(NotificarLeituraNotificacaoCommand request, CancellationToken cancellationToken)
        {
            var usuarioRf = request.UsuarioRf ?? await mediator.Send(new ObterUsuarioRfPorIdQuery(request.Notificacao.UsuarioId.Value));

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNotificacoes.Leitura,
                                                           new MensagemLeituraNotificacaoDto(request.Notificacao.Codigo,
                                                                                             usuarioRf,
                                                                                             request.Notificacao.AnoAnterior)));
        }
    }
}

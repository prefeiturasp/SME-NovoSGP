using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Notificacoes.Hub.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarCriacaoNotificacaoCommandHandler : AsyncRequestHandler<NotificarCriacaoNotificacaoCommand>
    {
        private readonly IMediator mediator;

        public NotificarCriacaoNotificacaoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(NotificarCriacaoNotificacaoCommand request, CancellationToken cancellationToken)
        {
            var usuarioRf = string.IsNullOrEmpty(request.UsuarioRf) ?
                await mediator.Send(new ObterUsuarioRfPorIdQuery(request.Notificacao.UsuarioId.Value)) :
                request.UsuarioRf;

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNotificacoes.Criacao,
                                                           new MensagemCriacaoNotificacaoDto(
                                                               request.Notificacao.Id,
                                                               request.Notificacao.Codigo,
                                                               request.Notificacao.Titulo,
                                                               request.Notificacao.CriadoEm,
                                                               usuarioRf)));
        }
    }
}

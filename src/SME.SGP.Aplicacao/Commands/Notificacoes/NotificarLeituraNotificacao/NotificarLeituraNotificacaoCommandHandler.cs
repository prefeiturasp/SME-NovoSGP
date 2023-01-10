using MediatR;
using Microsoft.Win32;
using Polly;
using Polly.Registry;
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
        private readonly IAsyncPolicy policy;

        public NotificarLeituraNotificacaoCommandHandler(IMediator mediator, IReadOnlyPolicyRegistry<string> registry)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.policy = registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila);
        }

        protected override async Task Handle(NotificarLeituraNotificacaoCommand request, CancellationToken cancellationToken)
        {
            var usuarioRf = await policy.ExecuteAsync(() => mediator.Send(new ObterUsuarioRfPorIdQuery(request.Notificacao.UsuarioId.Value)));

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNotificacoes.Leitura,
                                                           new MensagemLeituraNotificacaoDto(request.Notificacao.Codigo,
                                                                                             usuarioRf,
                                                                                             request.Notificacao.AnoAnterior)));
        }
    }
}

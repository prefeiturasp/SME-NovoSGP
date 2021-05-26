using MediatR;
using Sentry;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoConclusaoEncaminhamentoAEECommandHandler : IRequestHandler<ExecutaNotificacaoConclusaoEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoConclusaoEncaminhamentoAEECommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoConclusaoEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            SentrySdk.AddBreadcrumb($"Mensagem NotificacaoConclusaoEncaminhamentoAEEUseCase", "Rabbit - NotificacaoConclusaoEncaminhamentoAEEUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoRegistroConclusaoEncaminhamentoAEE,
                new NotificacaoEncaminhamentoAEEDto
                {
                    EncaminhamentoAEEId = request.EncaminhamentoAEEId,
                    UsuarioRF = request.UsuarioRF,
                    UsuarioNome = request.UsuarioNome
                }, Guid.NewGuid(), null));

            return true;
        }
    }
}

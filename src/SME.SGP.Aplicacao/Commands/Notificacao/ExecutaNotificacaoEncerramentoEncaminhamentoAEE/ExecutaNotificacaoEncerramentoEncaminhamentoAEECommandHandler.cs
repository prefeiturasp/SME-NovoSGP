using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoEncerramentoEncaminhamentoAEECommandHandler : IRequestHandler<ExecutaNotificacaoEncerramentoEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoEncerramentoEncaminhamentoAEECommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoEncerramentoEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoEncerramentoEncaminhamentoAEE,
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

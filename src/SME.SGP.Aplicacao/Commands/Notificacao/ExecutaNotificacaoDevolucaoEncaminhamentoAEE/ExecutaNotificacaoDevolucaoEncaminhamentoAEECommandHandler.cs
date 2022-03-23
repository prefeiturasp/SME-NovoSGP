using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoDevolucaoEncaminhamentoAEECommandHandler : IRequestHandler<ExecutaNotificacaoDevolucaoEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoDevolucaoEncaminhamentoAEECommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoDevolucaoEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoDevolucaoEncaminhamentoAEE,
                new NotificacaoEncaminhamentoAEEDto
                {
                    EncaminhamentoAEEId = request.EncaminhamentoAEEId,
                    UsuarioRF = request.UsuarioRF,
                    UsuarioNome = request.UsuarioNome,
                    Motivo = request.MotivoDevolucao
                }, Guid.NewGuid(), null));

            return true;
        }
    }
}

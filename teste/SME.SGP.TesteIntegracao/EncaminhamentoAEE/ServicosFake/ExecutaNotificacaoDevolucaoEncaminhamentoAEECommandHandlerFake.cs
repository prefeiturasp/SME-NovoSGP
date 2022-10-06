using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake
{
    public class ExecutaNotificacaoDevolucaoEncaminhamentoAEECommandHandlerFake : IRequestHandler<ExecutaNotificacaoDevolucaoEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoDevolucaoEncaminhamentoAEECommandHandlerFake(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoDevolucaoEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            if (request.EncaminhamentoAEEId > 0)
                await mediator.Send(new NotificacaoDevolucaoEncaminhamentoAEECommand(request.EncaminhamentoAEEId, request.UsuarioRF, request.UsuarioNome, request.MotivoDevolucao));

            return true;
        }
    }
}

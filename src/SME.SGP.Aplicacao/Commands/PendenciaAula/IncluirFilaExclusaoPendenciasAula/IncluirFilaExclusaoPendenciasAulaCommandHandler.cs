using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaExclusaoPendenciasAulaCommandHandler : IRequestHandler<IncluirFilaExclusaoPendenciasAulaCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaExclusaoPendenciasAulaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaExclusaoPendenciasAulaCommand request, CancellationToken cancellationToken)
        {
            var command = new ExcluirTodasPendenciasAulaCommand(request.AulaId);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaExclusaoPendenciasAula, command, Guid.NewGuid(), request.Usuario));
            SentrySdk.AddBreadcrumb($"Incluir fila exclusão pendências aula", "RabbitMQ");

            return true;
        }
    }
}

using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaAlteracaoAulaRecorrenteCommandHandler : IRequestHandler<IncluirFilaAlteracaoAulaRecorrenteCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaAlteracaoAulaRecorrenteCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaAlteracaoAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            var command = new AlterarAulaRecorrenteCommand(request);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaAlterarAulaRecorrencia, command, Guid.NewGuid(), request.Usuario));

            return true;
        }
    }
}

using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaExclusaoAulaRecorrenteCommandHandler : IRequestHandler<IncluirFilaExclusaoAulaRecorrenteCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaExclusaoAulaRecorrenteCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaExclusaoAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            var command = new ExcluirAulaRecorrenteCommand(request.AulaId,
                                                           request.Recorrencia,
                                                           request.ComponenteCurricularNome,
                                                           request.Usuario);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExcluirAulaRecorrencia, command, Guid.NewGuid(), request.Usuario, true));

            return true;
        }
    }
}

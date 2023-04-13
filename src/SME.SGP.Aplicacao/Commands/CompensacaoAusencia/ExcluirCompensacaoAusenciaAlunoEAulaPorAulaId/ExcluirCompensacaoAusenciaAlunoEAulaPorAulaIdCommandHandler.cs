using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommandHandler : IRequestHandler<ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand, bool>
    {
        private readonly IMediator mediator;

        public ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand request, CancellationToken cancellationToken)
        {
            var compensacaoAusenciaAlunoAulas = await mediator.Send(new ObterCompensacaoAusenciaAlunoAulaPorAulaIdQuery(request.AulaId, request.NumeroAula), cancellationToken);
            if (compensacaoAusenciaAlunoAulas.Any())
                await mediator.Send(new ExcluirCompensacaoAusenciaEAlunoEAulaCommand(compensacaoAusenciaAlunoAulas), cancellationToken);

            return true;
        }
    }
}


using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaExclusaoPendenciasAusenciaFechamentoCommandHandler : IRequestHandler<VerificaExclusaoPendenciasAusenciaFechamentoCommand, bool>
    {
        private readonly IMediator mediator;

        public VerificaExclusaoPendenciasAusenciaFechamentoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificaExclusaoPendenciasAusenciaFechamentoCommand request, CancellationToken cancellationToken)
        {
            var pendenciaFechamentos = await mediator.Send(new ObterPendenciasFechamentoIdDisciplinaQuery(request.FechamentoId, request.DisciplinaId));

            foreach (var pendenciaFechamento in pendenciaFechamentos)
            {
                await mediator.Send(new ExcluirPendenciaFechamentoCommand(pendenciaFechamento));
            }

            return true;
        }
    }
}

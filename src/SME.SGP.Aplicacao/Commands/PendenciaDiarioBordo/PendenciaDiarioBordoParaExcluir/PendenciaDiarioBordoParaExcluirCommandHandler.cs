using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaDiarioBordoParaExcluirCommandHandler : IRequestHandler<PendenciaDiarioBordoParaExcluirCommand, bool>
    {
        private readonly IMediator _mediator;

        public PendenciaDiarioBordoParaExcluirCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<bool> Handle(PendenciaDiarioBordoParaExcluirCommand request, CancellationToken cancellationToken)
        {
            var todosRetornaramSucesso = true;
            foreach (var item in request.PendenciaDiariosBordoParaExcluirDto)
            {
                var command = new ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand(item.AulaId, item.ComponenteCurricularId);
                var result = await _mediator.Send(command, cancellationToken);

                if (!result && todosRetornaramSucesso) todosRetornaramSucesso = false;
            }
            return todosRetornaramSucesso;
        }
    }
}
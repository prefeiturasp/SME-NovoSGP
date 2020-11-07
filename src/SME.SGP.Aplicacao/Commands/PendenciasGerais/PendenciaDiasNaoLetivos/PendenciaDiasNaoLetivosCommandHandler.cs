using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaDiasNaoLetivosCommandHandler : IRequestHandler<PendenciaDiasNaoLetivosCommand, bool>
    {
        public PendenciaDiasNaoLetivosCommandHandler()
        {

        }

        public Task<bool> Handle(PendenciaDiasNaoLetivosCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}

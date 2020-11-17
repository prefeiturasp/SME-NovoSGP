using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaEventoCalendarioCommandHandler : IRequestHandler<PendenciaEventoCalendarioCommand, bool>
    {
        public PendenciaEventoCalendarioCommandHandler()
        {

        }

        public Task<bool> Handle(PendenciaEventoCalendarioCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}

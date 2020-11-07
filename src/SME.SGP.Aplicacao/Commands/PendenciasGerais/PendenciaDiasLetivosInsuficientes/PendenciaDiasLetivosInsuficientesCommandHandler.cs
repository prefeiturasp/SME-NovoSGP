using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaDiasLetivosInsuficientesCommandHandler : IRequestHandler<PendenciaDiasLetivosInsuficientesCommand, bool>
    {
        public PendenciaDiasLetivosInsuficientesCommandHandler()
        {

        }

        public Task<bool> Handle(PendenciaDiasLetivosInsuficientesCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}

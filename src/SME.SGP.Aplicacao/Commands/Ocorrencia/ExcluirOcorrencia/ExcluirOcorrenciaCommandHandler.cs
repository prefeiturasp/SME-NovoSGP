using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirOcorrenciaCommandHandler : IRequestHandler<ExcluirOcorrenciaCommand, bool>
    {
        public Task<bool> Handle(ExcluirOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}

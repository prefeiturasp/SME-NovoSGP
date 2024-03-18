using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ObterPendenciasAulaPorAulaIdsQueryHandlerFake : IRequestHandler<ObterPendenciasAulaPorAulaIdsQuery, bool>
    {

        public ObterPendenciasAulaPorAulaIdsQueryHandlerFake()
        { }

        public Task<bool> Handle(ObterPendenciasAulaPorAulaIdsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}

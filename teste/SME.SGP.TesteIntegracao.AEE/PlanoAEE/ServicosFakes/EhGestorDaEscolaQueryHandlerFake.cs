using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class EhGestorDaEscolaQueryHandlerFake : IRequestHandler<EhGestorDaEscolaQuery, bool>
    {
        public Task<bool> Handle(EhGestorDaEscolaQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}

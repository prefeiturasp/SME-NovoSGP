using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes.Query
{
    public class PodePersistirTurmaDisciplinaQueryHandlerFakeRetornaFalso : IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>
    {
        public async Task<bool> Handle(PodePersistirTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(false);
        }
    }
}

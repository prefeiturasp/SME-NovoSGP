using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class PodePersistirTurmaDisciplinaQueryHandlerFake : IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>
    {
        public async Task<bool> Handle(PodePersistirTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return !request.ComponenteParaVerificarAtribuicao.Equals("139");
        }
    }
}

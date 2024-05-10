using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterTotalAulasPorDisciplinaETurmaQueryFake : IRequestHandler<ObterTotalAulasPorDisciplinaETurmaQuery, int>
    {
        public async Task<int> Handle(ObterTotalAulasPorDisciplinaETurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(2);
        }
    }
}

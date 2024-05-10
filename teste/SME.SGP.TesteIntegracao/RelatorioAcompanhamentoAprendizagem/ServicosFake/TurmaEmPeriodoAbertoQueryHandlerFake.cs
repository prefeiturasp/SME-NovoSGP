using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFakes
{
    public class TurmaEmPeriodoAbertoQueryHandlerFake : IRequestHandler<TurmaEmPeriodoAbertoQuery, bool>
    {
        public async Task<bool> Handle(TurmaEmPeriodoAbertoQuery request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}
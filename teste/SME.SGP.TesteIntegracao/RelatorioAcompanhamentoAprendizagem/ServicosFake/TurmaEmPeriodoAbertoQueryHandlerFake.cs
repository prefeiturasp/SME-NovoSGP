using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

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
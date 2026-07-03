using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFake
{
    public class TurmaEmPeriodoAbertoQueryFake : IRequestHandler<TurmaEmPeriodoAbertoQuery, bool>
    {
        public static bool ShouldReturnPeriodoAberto { get; private set; } = true;
        public static void SetShouldReturnPeriodoAberto(bool value)
        {
            ShouldReturnPeriodoAberto = value;
        }

        public Task<bool> Handle(TurmaEmPeriodoAbertoQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(ShouldReturnPeriodoAberto);
        }
    }
}

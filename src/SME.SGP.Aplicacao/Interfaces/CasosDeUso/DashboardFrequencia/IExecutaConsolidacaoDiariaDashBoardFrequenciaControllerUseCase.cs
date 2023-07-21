using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExecutaConsolidacaoDiariaDashBoardFrequenciaControllerUseCase
    {
        Task<bool> Executar(FiltroConsolicacaoDiariaDashBoardFrequenciaDto filtroConsolicacao);
    }
}
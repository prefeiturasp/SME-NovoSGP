using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase
    {
        Task<bool> Executar(FiltroConsolicacaoDiariaDashBoardFrequenciaDTO filtroConsolicacao);
    }
}
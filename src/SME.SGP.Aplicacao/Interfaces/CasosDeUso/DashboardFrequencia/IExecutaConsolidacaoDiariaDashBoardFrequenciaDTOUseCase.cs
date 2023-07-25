using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase
    {
        Task<bool> Executar(FiltroConsolicacaoDiariaDashBoardFrequenciaDTO filtroConsolicacao);
    }
}
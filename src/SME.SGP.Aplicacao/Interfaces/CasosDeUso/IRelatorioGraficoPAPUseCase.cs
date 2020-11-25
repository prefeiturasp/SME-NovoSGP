using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IRelatorioGraficoPAPUseCase
    {
        Task<bool> Executar(FiltroRelatorioResumoPAPDto filtroRelatorioResumoPAPDto);
    }
}

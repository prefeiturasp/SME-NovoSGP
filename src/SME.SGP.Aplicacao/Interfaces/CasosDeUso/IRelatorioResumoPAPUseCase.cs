using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IRelatorioResumoPAPUseCase
    {
        Task<bool> Executar(FiltroRelatorioResumoPAPDto filtroRelatorioResumoPAPDto);
    }
}

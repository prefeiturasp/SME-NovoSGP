using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IRelatorioConselhoClasseAtaFinalUseCase
    {
        Task<bool> Executar(FiltroRelatorioConselhoClasseAtaFinalDto filtroRelatorioConselhoClasseAtaFinalDto);
    }
}

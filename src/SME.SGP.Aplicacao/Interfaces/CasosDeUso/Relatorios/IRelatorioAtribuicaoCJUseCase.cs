using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IRelatorioAtribuicaoCJUseCase
    {
        Task<bool> Executar(FiltroRelatorioAtribuicaoCJDto filtroRelatorioAtribuicaoCJDto);
    }
}

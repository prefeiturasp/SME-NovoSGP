using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IBoletimUseCase
    {
        Task<bool> Executar(FiltroRelatorioBoletimDto filtroRelatorioBoletimDto);
    }
}

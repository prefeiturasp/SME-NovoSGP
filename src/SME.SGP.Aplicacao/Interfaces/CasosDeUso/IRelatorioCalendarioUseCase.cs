using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IRelatorioCalendarioUseCase
    {
        Task<bool> Executar(FiltroRelatorioCalendarioDto filtroRelatorioCalendarioDto);
    }
}

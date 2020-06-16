using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IGamesUseCase
    {
        Task<bool> Executar(FiltroRelatorioGamesDto filtroRelatorioGamesDto);
    }
}

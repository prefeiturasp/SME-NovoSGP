using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IRelatorioPlanejamentoAnualUseCase
    {
        Task<bool> Executar(FiltroRelatorioPlanejamentoAnualDto filtro);
    }
}
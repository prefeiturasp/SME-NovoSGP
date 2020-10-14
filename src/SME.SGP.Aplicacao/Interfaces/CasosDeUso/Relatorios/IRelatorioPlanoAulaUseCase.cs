using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IRelatorioPlanoAulaUseCase
    {
        Task<bool> Executar(FiltroRelatorioPlanoAulaDto filtro);
    }
}
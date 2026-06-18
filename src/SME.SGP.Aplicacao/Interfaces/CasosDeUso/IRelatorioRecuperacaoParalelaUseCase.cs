using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioRecuperacaoParalelaUseCase
    {
        Task<bool> Executar(FiltroRelatorioRecuperacaoParalelaDto filtro);
    }
}

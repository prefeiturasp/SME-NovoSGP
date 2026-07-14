using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioRecuperacaoParalelaUseCase
    {
        Task<bool> Executar(FiltroRelatorioRecuperacaoParalelaDto filtro);
    }
}

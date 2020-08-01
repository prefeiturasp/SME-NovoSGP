using System.Threading.Tasks;
using SME.SGP.Infra.Dtos.Relatorios.RecuperacaoParalela;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioRecuperacaoParalelaUseCase
    {
        Task<bool> Executar(FiltroRelatorioRecuperacaoParalelaDto filtro);
    }
}

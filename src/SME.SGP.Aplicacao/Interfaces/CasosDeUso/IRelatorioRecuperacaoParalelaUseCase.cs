using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioRecuperacaoParalelaUseCase
    {
        Task<bool> Executar(FiltroRelatorioRecuperacaoParalelaDto filtro);
    }
}

using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarCompensacaoAusenciaUseCase
    {
        Task Executar(long id, CompensacaoAusenciaDto compensacaoDto);
    }
}

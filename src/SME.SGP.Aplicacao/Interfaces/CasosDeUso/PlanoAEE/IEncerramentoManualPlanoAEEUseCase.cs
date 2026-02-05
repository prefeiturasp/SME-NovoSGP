using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IEncerramentoManualPlanoAEEUseCase
    {
        Task<RetornoEncerramentoPlanoAEEDto> Executar(long planoId);
    }
}

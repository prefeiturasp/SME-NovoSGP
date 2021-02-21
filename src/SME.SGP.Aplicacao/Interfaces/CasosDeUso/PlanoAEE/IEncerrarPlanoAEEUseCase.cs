using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IEncerrarPlanoAEEUseCase
    {
        Task<RetornoEncerramentoPlanoAEEDto> Executar(long planoId);
    }
}

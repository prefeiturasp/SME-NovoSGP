using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IEnviarParaAnaliseEncaminhamentoAEEUseCase
    {
        Task<bool> Executar(long encaminhamentoId);
    }
}

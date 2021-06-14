using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IEncerrarEncaminhamentoAEEUseCase
    {
        Task<bool> Executar(long encaminhamentoId, string motivoEncerramento);
    }
}

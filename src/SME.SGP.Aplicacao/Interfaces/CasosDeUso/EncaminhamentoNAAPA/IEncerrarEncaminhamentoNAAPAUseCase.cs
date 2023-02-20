using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IEncerrarEncaminhamentoNAAPAUseCase
    {
        Task<bool> Executar(long encaminhamentoId, string motivoEncerramento);
    }
}

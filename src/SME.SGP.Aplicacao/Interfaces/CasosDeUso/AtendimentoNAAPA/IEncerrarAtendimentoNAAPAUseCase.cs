using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IEncerrarAtendimentoNAAPAUseCase
    {
        Task<bool> Executar(long encaminhamentoId, string motivoEncerramento);
    }
}

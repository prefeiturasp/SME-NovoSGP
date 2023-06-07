using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IExcluirObservacoesDeEncaminhamentoNAAPAUseCase
    {
        Task<bool> Executar(long observacaoId);
    }
}
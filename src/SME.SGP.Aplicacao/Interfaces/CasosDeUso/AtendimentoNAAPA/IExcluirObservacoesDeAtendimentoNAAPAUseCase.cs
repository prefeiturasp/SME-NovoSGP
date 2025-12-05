using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IExcluirObservacoesDeAtendimentoNAAPAUseCase
    {
        Task<bool> Executar(long observacaoId);
    }
}
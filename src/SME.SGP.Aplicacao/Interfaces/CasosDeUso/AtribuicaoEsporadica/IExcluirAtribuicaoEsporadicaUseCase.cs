using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IExcluirAtribuicaoEsporadicaUseCase
    {
        Task<bool> Executar(long id);
    }
}
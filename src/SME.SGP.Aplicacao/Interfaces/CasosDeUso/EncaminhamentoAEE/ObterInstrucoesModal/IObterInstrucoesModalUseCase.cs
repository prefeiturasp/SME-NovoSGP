using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterInstrucoesModalUseCase
    {
        Task<string> Executar();
    }
}

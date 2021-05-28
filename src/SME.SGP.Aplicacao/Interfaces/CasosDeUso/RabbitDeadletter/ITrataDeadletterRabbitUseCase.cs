using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface ITrataDeadletterRabbitUseCase
    {
        Task<bool> Executar();
    }
}

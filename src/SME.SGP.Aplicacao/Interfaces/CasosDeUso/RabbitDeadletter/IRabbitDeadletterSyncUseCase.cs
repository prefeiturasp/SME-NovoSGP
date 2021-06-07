using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IRabbitDeadletterSyncUseCase
    {
        Task<bool> Executar();
    }
}

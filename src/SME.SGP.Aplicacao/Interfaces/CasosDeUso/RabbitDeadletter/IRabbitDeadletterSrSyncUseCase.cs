using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IRabbitDeadletterSrSyncUseCase
    {
        Task<bool> Executar();
    }
}

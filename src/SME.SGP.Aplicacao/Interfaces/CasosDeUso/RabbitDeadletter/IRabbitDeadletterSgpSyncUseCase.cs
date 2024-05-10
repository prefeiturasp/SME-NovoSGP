using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IRabbitDeadletterSgpSyncUseCase
    {
        Task<bool> Executar();
    }
}

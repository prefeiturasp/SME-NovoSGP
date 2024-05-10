using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IRabbitDeadletterSerapSyncUseCase
    {
        Task<bool> Executar();
    }
}

using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioItineranciaQuestao : IRepositorioBase<ItineranciaQuestao>
    {
        Task ExcluirItineranciaQuestao(long questaoId, long itineranciaId);
    }
}
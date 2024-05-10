using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioItineranciaObjetivo : IRepositorioBase<ItineranciaObjetivo>
    {
        Task ExcluirItineranciaObjetivo(long objetivoId, long itineranciaId);
    }
}
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioItineranciaUe : IRepositorioBase<ItineranciaUe>
    {
        Task ExcluirItineranciaUe(long ueId, long itineranciaId);
    }
}
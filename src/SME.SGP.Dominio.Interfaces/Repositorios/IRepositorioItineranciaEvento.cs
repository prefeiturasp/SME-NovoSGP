using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioItineranciaEvento : IRepositorioBase<ItineranciaEvento>
    {
        Task<IEnumerable<long>> ObterEventosIdsPorItinerancia(long itineranciaId);
        Task<IEnumerable<Evento>> ObterEventosPorItineranciaId(long itineranciaId);
    }
}

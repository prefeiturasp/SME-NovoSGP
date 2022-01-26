using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoTurmaItinerario
    {
        Task<IEnumerable<TipoTurmaItinerario>> ObterPorSerie(int serie);

        Task<TipoTurmaItinerario> ObterPorId(int id);
    }
}
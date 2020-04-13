using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseRecomendacao : IRepositorioBase<ConselhoClasseRecomendacao>
    {
        Task<IEnumerable<ConselhoClasseRecomendacao>> ObterTodosAsync();
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComponenteCurricular
    {
        Task<IEnumerable<ComponenteCurricular>> ObterPorIdsAsync(long[] ids);
    }
}

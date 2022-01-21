using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioBaseEntity<T> where T : EntidadeBase
    {
        Task InserirVariosAsync(IEnumerable<T> entidades);
        Task AlterarVariosAsync(IEnumerable<T> entidades);
    }
}

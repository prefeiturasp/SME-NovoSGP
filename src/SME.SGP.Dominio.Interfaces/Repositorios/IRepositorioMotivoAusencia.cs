using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioMotivoAusencia
    {
        Task<IEnumerable<MotivoAusencia>> Listar();
    }
}

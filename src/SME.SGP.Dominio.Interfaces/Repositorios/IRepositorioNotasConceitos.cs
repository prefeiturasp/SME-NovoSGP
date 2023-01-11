using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotasConceitos : IRepositorioBase<NotaConceito>
    {
        Task<bool> SalvarListaNotaConceito(IEnumerable<NotaConceito> entidade, Usuario criadoPor);
    }
}
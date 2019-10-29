using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoCalendario : IRepositorioBase<TipoCalendario>
    {
        IEnumerable<TipoCalendario> ObterTiposCalendario();

        Task<bool> VerificarRegistroExistente(long id, string nome);
    }
}
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUsuario : IRepositorioBase<Usuario>
    {
        Task AtualizarUltimoLogin(long id, DateTime ultimoLogin);

        Task<IEnumerable<Usuario>> ObterPorIdsAsync(long[] ids);
    }
}
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUsuario : IRepositorioBase<Usuario>
    {
        Task AtualizarUltimoLogin(long id, DateTime ultimoLogin);
    }
}
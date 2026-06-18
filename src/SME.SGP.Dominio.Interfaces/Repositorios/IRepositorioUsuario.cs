using SME.SGP.Infra.Dtos.Abrangencia;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUsuario : IRepositorioBase<Usuario>
    {
        Task AtualizarUltimoLogin(long id, DateTime ultimoLogin);

        Task<IEnumerable<Usuario>> ObterPorIdsAsync(long[] ids);
        Task<IEnumerable<AbrangenciaUsuarioPerfilDto>> ObterUsuariosPerfis();
    }
}
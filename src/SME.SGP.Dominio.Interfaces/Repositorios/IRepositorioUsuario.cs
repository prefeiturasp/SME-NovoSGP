using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SME.SGP.Infra.Dtos.Abrangencia;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUsuario : IRepositorioBase<Usuario>
    {
        Task AtualizarUltimoLogin(long id, DateTime ultimoLogin);

        Task<IEnumerable<Usuario>> ObterPorIdsAsync(long[] ids);
        Task<IEnumerable<AbrangenciaUsuarioPerfilDto>> ObterUsuariosPerfis();
    }
}
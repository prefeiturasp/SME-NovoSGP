using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPrioridadePerfil : IRepositorioBase<PrioridadePerfil>
    {
        IEnumerable<PrioridadePerfil> ObterPerfisPorIds(IEnumerable<Guid> idsPerfis);
        Task<int> ObterTipoPerfil(Guid perfil);

        Task<IEnumerable<PrioridadePerfil>> ObterPerfisPorTipo(int tipo);
        Task<IEnumerable<PrioridadePerfil>> ObterHierarquiaPerfisPorPerfil(Guid perfilUsuario);
        Task<PrioridadePerfil> ObterPerfilPorId(Guid perfil);
    }
}
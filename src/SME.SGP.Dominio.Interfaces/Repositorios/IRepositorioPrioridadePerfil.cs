using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPrioridadePerfil : IRepositorioBase<PrioridadePerfil>
    {
        IEnumerable<PrioridadePerfil> ObterPerfisPorIds(IEnumerable<Guid> idsPerfis);
    }
}
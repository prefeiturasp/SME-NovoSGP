using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaUsuario : IRepositorioBase<PendenciaUsuario>
    {
        Task ExcluirPorPendenciaId(long pendenciaId);
    }
}

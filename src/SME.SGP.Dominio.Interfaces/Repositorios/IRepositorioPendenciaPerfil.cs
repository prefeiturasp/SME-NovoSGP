using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioPendenciaPerfil : IRepositorioBase<PendenciaPerfil>
    {
        Task<IEnumerable<PendenciaPerfil>> ObterPorPendenciaId(long pendenciaId);
        Task<bool> Excluir(long id);
    }
}

using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaUsuario : IRepositorioBase<PendenciaUsuario>
    {
        Task ExcluirPorPendenciaId(long pendenciaId);
        Task ExcluirPorPendenciaIdEUsuario(long pendenciaId, long usuarioId);
        Task AlteraUsuarioDaPendencia(long pendenciaId, long usuarioId);
    }
}

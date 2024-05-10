using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaUsuarioConsulta : IRepositorioBase<PendenciaUsuario>
    {
        Task<bool> ObterPendenciasUsuarioPorPendenciaUsuarioId(long usuarioId, long pendenciaId);
    }
}

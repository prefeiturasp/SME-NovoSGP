using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioPendenciaPerfilUsuario : IRepositorioBase<PendenciaPerfilUsuario>
    {
        Task<IEnumerable<PendenciaPerfilUsuarioDto>> ObterPorSituacao(int situacaoPendencia);
        Task<bool> ExcluirAsync(long id);
        Task<IEnumerable<long>> VerificaExistencia(long pendenciaPerfilId, long usuarioId);
    }
}

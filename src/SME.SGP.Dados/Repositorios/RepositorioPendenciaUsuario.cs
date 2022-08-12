using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaUsuario : RepositorioBase<PendenciaUsuario>, IRepositorioPendenciaUsuario
    {
        public RepositorioPendenciaUsuario(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task AlteraUsuarioDaPendencia(long pendenciaId, long usuarioId)
        {
            var command = @"update pendencia_usuario set usuario_id = @usuarioId where pendencia_id = @pendenciaId";

            await database.Conexao.ExecuteScalarAsync(command, new { usuarioId, pendenciaId });
        }

        public async Task ExcluirPorPendenciaId(long pendenciaPerfilId)
        {
            var command = @"delete from pendencia_perfil_usuario where pendencia_perfil_id = @pendenciaPerfilId";

            await database.Conexao.ExecuteScalarAsync(command, new { pendenciaPerfilId });
        }

        public async Task ExcluirPorPendenciaIdEUsuario(long pendenciaId, long usuarioId)
        {
            var command = @"delete from pendencia_usuario where pendencia_id = @pendenciaId and usuario_id = @usuarioId";
            await database.Conexao.ExecuteScalarAsync(command, new { pendenciaId, usuarioId });
        }
    }
}

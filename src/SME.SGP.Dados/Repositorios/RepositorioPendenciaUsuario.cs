using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaUsuario : RepositorioBase<PendenciaUsuario>, IRepositorioPendenciaUsuario
    {
        public RepositorioPendenciaUsuario(ISgpContext database) : base(database)
        {
        }

        public async Task ExcluirPorPendenciaId(long pendenciaId)
        {
            var command = @"delete from pendencia_usuario where pendencia_id = @pendenciaId";

            await database.Conexao.ExecuteScalarAsync(command, new { pendenciaId });
        }
    }
}

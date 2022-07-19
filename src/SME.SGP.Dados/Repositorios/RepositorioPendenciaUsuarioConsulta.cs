using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaUsuarioConsulta : RepositorioBase<PendenciaUsuario>, IRepositorioPendenciaUsuarioConsulta
    {
        public RepositorioPendenciaUsuarioConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<bool> ObterPendenciasUsuarioPorPendenciaUsuarioId(long usuarioId, long pendenciaId)
        {
            var query = @"select 0 
                          from pendencia_usuario 
                          where usuario_id = @usuarioId
                                and pendencia_id = @pendenciaId";

            var retorno = await database.Conexao.QueryAsync<int>(query, new { usuarioId, pendenciaId });
            
            return retorno.Any();
        }
    }
}

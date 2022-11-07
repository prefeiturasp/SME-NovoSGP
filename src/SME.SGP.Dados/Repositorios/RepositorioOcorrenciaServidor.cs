using System.Collections.Generic;
using System.Linq;
using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrenciaServidor : RepositorioBase<OcorrenciaServidor>, IRepositorioOcorrenciaServidor
    {
        public RepositorioOcorrenciaServidor(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task ExcluirPorOcorrenciaAsync(long idOcorrencia)
        {
            var sql = "delete from ocorrencia_servidor where ocorrencia_id = @idOcorrencia";
            await database.Conexao.ExecuteAsync(sql, new { idOcorrencia });
        }
        
        public async Task ExcluirPoIds(IEnumerable<long> ids)
        {
            if (!ids?.Any() ?? true) return;
            
            var sql = "delete from ocorrencia_servidor where id = any(@ids)";
            await database.Conexao.ExecuteAsync(sql, new { ids = ids.ToList() });
        }

        public async Task<IEnumerable<OcorrenciaServidor>> ObterPorIdOcorrencia(long idOcorrencia)
        {
            var sql = "select * from ocorrencia_servidor where ocorrencia_id = @idOcorrencia";
            return await database.Conexao.QueryAsync<OcorrenciaServidor>(sql,new{idOcorrencia});
        }
    }
}

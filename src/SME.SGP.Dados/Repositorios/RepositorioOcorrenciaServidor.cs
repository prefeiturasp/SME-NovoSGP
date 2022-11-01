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
    }
}

using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEE : RepositorioBase<PlanoAEE>, IRepositorioPlanoAEE
    {
        public RepositorioPlanoAEE(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<int> AtualizarSituacaoPlanoPorVersao(long versaoId, int situacao)
        {
            var query = @"update plano_aee
                           set situacao = @situacao
                          where id in (select plano_aee_id from plano_aee_versao where id = @versaoId) ";

            return await database.Conexao.ExecuteAsync(query, new { versaoId, situacao });
        }        
    }
}

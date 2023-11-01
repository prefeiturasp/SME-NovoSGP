using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioInformativoNotificacao : RepositorioBase<InformativoNotificacao>, IRepositorioInformativoNotificacao
    {
        public RepositorioInformativoNotificacao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<long>> ObterIdsNotificacoesPorInformeIdAsync(long informeId)
        {
            var query = @$" select notificacao_id
                            from informativo_notificacao 
                            where informativo_id = @informeId and not excluido ";

            return await database.Conexao.QueryAsync<long>(query, new { informeId });
        }

        public async Task<bool> RemoverLogicoPorInformeIdAsync(long informeId)
        {
            var query = @"update informativo_notificacao set excluido = true where informativo_id = @informeId";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { informeId });
        }
    }
}

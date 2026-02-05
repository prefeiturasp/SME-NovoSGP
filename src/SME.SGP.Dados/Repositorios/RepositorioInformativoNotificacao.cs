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

        public async Task<long> ObterIdInformativoPorNotificacaoIdAsync(long notificacaoId)
        {
            var query = @$" select informativo_id
                            from informativo_notificacao 
                            where notificacao_id = @notificacaoId ";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { notificacaoId });
        }

        public async Task<IEnumerable<long>> ObterIdsNotificacoesPorInformativoIdAsync(long informativoId)
        {
            var query = @$" select notificacao_id
                            from informativo_notificacao 
                            where informativo_id = @informativoId ";

            return await database.Conexao.QueryAsync<long>(query, new { informativoId });
        }

        public async Task<bool> RemoverLogicoPorInformativoIdAsync(long informativoId)
        {
            var query = @"update informativo_notificacao set excluido = true where informativo_id = @informativoId";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { informativoId });
        }

        public async Task<bool> VerificaSeExisteNotificacaoInformePorIdUsuarioRfAsync(long informativoId, string usuarioRf)
        {
            var query = @$"select
	                            count(n.id) > 0
                            from
	                            informativo_notificacao t
                            inner join notificacao n on
	                            n.id = t.notificacao_id
                            inner join usuario u on
	                            u.id = n.usuario_id
                            where
	                            t.informativo_id = @informativoId
	                            and u.login = @usuarioRf
	                            and n.excluida = false
	                            and t.excluido = false";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { informativoId, usuarioRf });
        }
    }
}

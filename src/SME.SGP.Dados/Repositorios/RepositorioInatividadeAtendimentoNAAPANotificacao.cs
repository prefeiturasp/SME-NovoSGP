using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioInatividadeAtendimentoNAAPANotificacao : RepositorioBase<InatividadeAtendimentoNAAPANotificacao>, IRepositorioInatividadeAtendimentoNAAPANotificacao
    {
        public RepositorioInatividadeAtendimentoNAAPANotificacao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<long>> ObterIdsNotificacoesPorNAAPAIdAsync(long encaminhamentoNAAPAId)
        {
            var query = @$" select notificacao_id
                            from inatividade_atendimento_naapa_notificacao 
                            where encaminhamento_naapa_id = @encaminhamentoNAAPAId ";

            return await database.Conexao.QueryAsync<long>(query, new { encaminhamentoNAAPAId });
        }

        public async Task<bool> RemoverLogicoPorNAAPAIdAsync(long encaminhamentoNAAPAId)
        {
            var query = @"update inatividade_atendimento_naapa_notificacao set excluido = true where encaminhamento_naapa_id = @encaminhamentoNAAPAId";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { encaminhamentoNAAPAId });
        }

        public async Task<bool> RemoverPorNAAPAIdAsync(long encaminhamentoNAAPAId)
        {
            var query = @"delete from inatividade_atendimento_naapa_notificacao where encaminhamento_naapa_id = @encaminhamentoNAAPAId";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { encaminhamentoNAAPAId });
        }
    }
}

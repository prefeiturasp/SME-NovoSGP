using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioNotificacaoPlanoAEEObservacao : RepositorioBase<NotificacaoPlanoAEEObservacao>, IRepositorioNotificacaoPlanoAEEObservacao
    {
        public RepositorioNotificacaoPlanoAEEObservacao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<NotificacaoPlanoAEEObservacao>> ObterPorObservacaoPlanoAEEId(long observacaoPlanoId)
        {
            var query = "select * from notificacao_plano_aee_observacao where plano_aee_observacao_id = @observacaoPlanoId";

            return await database.Conexao.QueryAsync<NotificacaoPlanoAEEObservacao>(query, new { observacaoPlanoId });
        }
    }
}

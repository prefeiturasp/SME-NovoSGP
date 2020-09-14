using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacaoDevolutiva : IRepositorioNotificacaoDevolutiva
    {
        private readonly ISgpContext database;

        public RepositorioNotificacaoDevolutiva(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<NotificacaoDevolutiva>> ObterPorDevolutivaId(long devolutivaId)
        {
            var query = "select id, notificacao_id as NotificacaoId, devolutiva_id as DevolutivaId from devolutiva_diario_bordo_notificacao where devolutiva_id = @devolutivaId";

            return (await database.Conexao.QueryAsync<NotificacaoDevolutiva>(query, new { devolutivaId }));
        }

        public async Task Excluir(NotificacaoDevolutiva notificacao)
        {
            await database.Conexao.DeleteAsync(notificacao);
        }

        public async Task Salvar(NotificacaoDevolutiva notificacao)
        {
            await database.Conexao.InsertAsync(notificacao);
        }

    }
}

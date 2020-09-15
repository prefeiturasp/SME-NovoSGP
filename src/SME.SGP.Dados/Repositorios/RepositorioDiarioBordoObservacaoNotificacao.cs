using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordoObservacaoNotificacao : IRepositorioDiarioBordoObservacaoNotificacao
    {
        private readonly ISgpContext database;

        public RepositorioDiarioBordoObservacaoNotificacao(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<DiarioBordoObservacaoNotificacao>> ObterPorDiarioBordoObservacaoId(long DiarioBordoObservacaoId)
        {
            var query = "select id, notificacao_id as NotificacaoId, observacao_id as DiarioBordoObservacaoId from diario_bordo_observacao_notificacao where observacao_id = @DiarioBordoObservacaoId";

            return (await database.Conexao.QueryAsync<DiarioBordoObservacaoNotificacao>(query, new { DiarioBordoObservacaoId }));
        }

        public async Task Excluir(DiarioBordoObservacaoNotificacao notificacao)
        {
            await database.Conexao.DeleteAsync(notificacao);
        }

        public async Task Salvar(DiarioBordoObservacaoNotificacao notificacao)
        {
            await database.Conexao.InsertAsync(notificacao);
        }

    }
}

using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioNotificacaoCartaIntencoesObservacao : IRepositorioNotificacaoCartaIntencoesObservacao
    {
        private readonly ISgpContext database;

        public RepositorioNotificacaoCartaIntencoesObservacao(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<NotificacaoCartaIntencoesObservacao>> ObterPorCartaIntencoesObservacaoId(long cartaIntencoesObservacaoId)
        {
            var query = "select id, notificacao_id as NotificacaoId, observacao_id as CartaIntencoesObservacaoId from carta_intencoes_observacao_notificacao where observacao_id = @cartaIntencoesObservacaoId";

            return (await database.Conexao.QueryAsync<NotificacaoCartaIntencoesObservacao>(query, new { cartaIntencoesObservacaoId }));
        }

        public async Task Excluir(NotificacaoCartaIntencoesObservacao notificacao)
        {
            await database.Conexao.DeleteAsync(notificacao);
        }

        public async Task Salvar(NotificacaoCartaIntencoesObservacao notificacao)
        {
            await database.Conexao.InsertAsync(notificacao);
        }

    }
}

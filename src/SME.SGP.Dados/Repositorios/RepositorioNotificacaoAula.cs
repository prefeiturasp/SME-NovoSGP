using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacaoAula : IRepositorioNotificacaoAula
    {
        private readonly ISgpContext database;

        public RepositorioNotificacaoAula(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task Inserir(long notificacaoId, long aulaId)
        {
            await database.Conexao.InsertAsync(new NotificacaoAula()
            {
                NotificacaoId = notificacaoId,
                AulaId = aulaId
            });
        }

        public async Task<IEnumerable<NotificacaoAula>> ObterPorAulaAsync(long aulaId)
            => await database.Conexao.QueryAsync<NotificacaoAula>("select * from notificacao_aula where aula_id = @aulaId", new { aulaId });

        public Task Excluir(NotificacaoAula notificacaoAula)
        {
            return database.Conexao.DeleteAsync(notificacaoAula);
        }
    }
}
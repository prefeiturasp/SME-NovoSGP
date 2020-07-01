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
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public RepositorioNotificacaoAula(ISgpContext database
                    , IRepositorioNotificacao repositorioNotificacao)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task Inserir(long notificacaoId, long aulaId)
        {
            await database.Conexao.InsertAsync(new NotificacaoAula()
            {
                NotificacaoId = notificacaoId,
                AulaId = aulaId
            });
        }

        public async Task Excluir(long aulaId)
        {
            foreach(var notificacaoAula in await ObterPorAulaAsync(aulaId))
            {
                repositorioNotificacao.Remover(notificacaoAula.NotificacaoId);
                database.Conexao.Delete(notificacaoAula);
            }
        }

        public async Task<IEnumerable<NotificacaoAula>> ObterPorAulaAsync(long aulaId)
            => await database.Conexao.QueryAsync<NotificacaoAula>("select * from notificacao_aula where aula_id = @aulaId", new { aulaId });
    }
}
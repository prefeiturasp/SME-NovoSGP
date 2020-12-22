using Dapper;
using Dommel;
using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;


namespace SME.SGP.Dados
{
    public class RepositorioNotificacaoCompensacaoAusencia : IRepositorioNotificacaoCompensacaoAusencia
    {
        private readonly ISgpContext database;
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public RepositorioNotificacaoCompensacaoAusencia(ISgpContext database
                    , IRepositorioNotificacao repositorioNotificacao)
        {
            this.database = database;
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        private IEnumerable<NotificacaoCompensacaoAusencia> ObterPorCompensacao(long compensacaoAusenciaId)
            => database.Conexao.Query<NotificacaoCompensacaoAusencia>("select * from notificacao_compensacao_ausencia where compensacao_ausencia_id = @compensacaoAusenciaId"
                            , new { compensacaoAusenciaId });

        public void Excluir(long compensacaoAusenciaId)
        {
            foreach(var notificacaoCompensacao in ObterPorCompensacao(compensacaoAusenciaId))
            {
                repositorioNotificacao.Remover(notificacaoCompensacao.NotificacaoId);
                database.Conexao.Delete(notificacaoCompensacao);                
            }

        }

        public void Inserir(long notificacaoId, long compensacaoAusenciaId)
        {
            database.Conexao.Insert(new NotificacaoCompensacaoAusencia()
            {
                NotificacaoId = notificacaoId,
                CompensacaoAusenciaId = compensacaoAusenciaId
            });
        }
    }
}

using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class RepositorioNotificacaoCartaIntencoesObservacao : IRepositorioNotificacaoCartaIntencoesObservacao
    {
        private readonly ISgpContext database;
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public RepositorioNotificacaoCartaIntencoesObservacao(ISgpContext database
                    , IRepositorioNotificacao repositorioNotificacao)
        {
            this.database = database;
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        private IEnumerable<NotificacaoCartaIntencoesObservacao> ObterPorObservacao(long cartaIntencoesObservacaoId)
            => database.Conexao.Query<NotificacaoCartaIntencoesObservacao>("select * from carta_intencoes_observacao_notificacao where observacao_id = @cartaIntencoesObservacaoId"
                            , new { cartaIntencoesObservacaoId });

        public void Excluir(long cartaIntencoesObservacaoId)
        {
            foreach(var notificacao in ObterPorObservacao(cartaIntencoesObservacaoId))
            {
                repositorioNotificacao.Remover(notificacao.NotificacaoId);
                database.Conexao.Delete(notificacao);
            }

        }

        public void Inserir(long notificacaoId, long cartaIntencoesObservacaoId)
        {
            database.Conexao.Insert(new NotificacaoCartaIntencoesObservacao()
            {
                NotificacaoId = notificacaoId,
                CartaIntencoesObservacaoId = cartaIntencoesObservacaoId
            });
        }
    }
}

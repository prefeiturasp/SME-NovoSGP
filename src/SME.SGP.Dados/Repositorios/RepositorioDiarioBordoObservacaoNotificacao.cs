using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordoObservacaoNotificacao : IRepositorioDiarioBordoObservacaoNotificacao
    {
        private readonly ISgpContext database;
        private readonly IRepositorioNotificacao repositorioNotificacao;
        public RepositorioDiarioBordoObservacaoNotificacao(ISgpContext database
                    , IRepositorioNotificacao repositorioNotificacao)
        {
            this.database = database;
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        private IEnumerable<DiarioBordoObservacaoNotificacao> ObterNotificacao(long idObservacao)
            => database.Conexao.Query<DiarioBordoObservacaoNotificacao>("select * from diario_bordo_observacao_notificacao where observacao_id = @idObservacao"
                            , new { idObservacao });

        public void Excluir(long idObservacao)
        {
            foreach (var notificacaoObservacao in ObterNotificacao(idObservacao))
            {
                repositorioNotificacao.Remover(notificacaoObservacao.IdNotificacao);
                database.Conexao.Delete(notificacaoObservacao);
            }
        }
    }
}

using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoCartaIntencoesObservacaoMap : SimpleEntityMap<NotificacaoCartaIntencoesObservacao>
    {
        public NotificacaoCartaIntencoesObservacaoMap()
        {
            ToTable("carta_intencoes_observacao_notificacao");
            Map(nameof(NotificacaoCartaIntencoesObservacao.NotificacaoId), "notificacao_id");
            Map(nameof(NotificacaoCartaIntencoesObservacao.CartaIntencoesObservacaoId), "observacao_id");
        }
    }
}
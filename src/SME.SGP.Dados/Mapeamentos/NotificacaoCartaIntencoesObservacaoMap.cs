using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoCartaIntencoesObservacaoMap : DommelEntityMap<NotificacaoCartaIntencoesObservacao>
    {
        public NotificacaoCartaIntencoesObservacaoMap()
        {
            ToTable("carta_intencoes_observacao_notificacao");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
            Map(c => c.CartaIntencoesObservacaoId).ToColumn("observacao_id");
        }
    }
}

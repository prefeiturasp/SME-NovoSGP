using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoPlanoAEEMap : BaseMap<NotificacaoPlanoAEE>
    {
        public NotificacaoPlanoAEEMap()
        {
            ToTable("notificacao_plano_aee");
            Map(nameof(NotificacaoPlanoAEE.Tipo), "tipo");
            Map(nameof(NotificacaoPlanoAEE.NotificacaoId), "notificacao_id");
            Map(nameof(NotificacaoPlanoAEE.PlanoAEEId), "plano_aee_id");
        }
    }
}
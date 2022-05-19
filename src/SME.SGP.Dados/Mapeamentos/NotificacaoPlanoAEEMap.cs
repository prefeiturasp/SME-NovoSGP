using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoPlanoAEEMap : BaseMap<NotificacaoPlanoAEE>
    {
        public NotificacaoPlanoAEEMap()
        {
            ToTable("notificacao_plano_aee");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
            Map(c => c.PlanoAEEId).ToColumn("plano_aee_id");
        }
    }
}
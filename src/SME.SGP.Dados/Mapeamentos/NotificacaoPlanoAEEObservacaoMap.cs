using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class NotificacaoPlanoAEEObservacaoMap : BaseMap<NotificacaoPlanoAEEObservacao>
    {
        public NotificacaoPlanoAEEObservacaoMap()
        {
            ToTable("notificacao_plano_aee_observacao");
            Map(a => a.PlanoAEEObservacaoId).ToColumn("plano_aee_observacao_id");
            Map(a => a.NotificacaoId).ToColumn("notificacao_id");
        }
    }
}

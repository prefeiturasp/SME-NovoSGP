using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class NotificacaoPlanoAEEObservacaoMap : BaseMap<NotificacaoPlanoAEEObservacao>
    {
        public NotificacaoPlanoAEEObservacaoMap()
        {
            ToTable("notificacao_plano_aee_observacao");
            Map(c => c.PlanoAEEObservacaoId).ToColumn("plano_aee_observacao_id");
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}

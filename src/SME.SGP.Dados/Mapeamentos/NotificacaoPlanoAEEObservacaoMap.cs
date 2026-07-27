using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class NotificacaoPlanoAEEObservacaoMap : BaseEntityMap<NotificacaoPlanoAEEObservacao>
    {
        public NotificacaoPlanoAEEObservacaoMap()
        {
            ToTable("notificacao_plano_aee_observacao");
            Map(nameof(NotificacaoPlanoAEEObservacao.PlanoAEEObservacaoId), "plano_aee_observacao_id");
            Map(nameof(NotificacaoPlanoAEEObservacao.NotificacaoId), "notificacao_id");
            Map(nameof(NotificacaoPlanoAEEObservacao.Excluido), "excluido");
        }
    }
}
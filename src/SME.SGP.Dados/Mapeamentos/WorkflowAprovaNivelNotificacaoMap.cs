using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovacaoNivelNotificacaoMap : SimpleEntityMap<WorkflowAprovacaoNivelNotificacao>
    {
        public WorkflowAprovacaoNivelNotificacaoMap()
        {
            ToTable("wf_aprovacao_nivel_notificacao");
            Map(nameof(WorkflowAprovacaoNivelNotificacao.NotificacaoId), "notificacao_id");
            Map(nameof(WorkflowAprovacaoNivelNotificacao.WorkflowAprovacaoNivelId), "wf_aprovacao_nivel_id");
        }
    }
}
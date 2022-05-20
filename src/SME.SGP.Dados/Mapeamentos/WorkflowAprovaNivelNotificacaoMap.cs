using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovacaoNivelNotificacaoMap : DommelEntityMap<WorkflowAprovacaoNivelNotificacao>
    {
        public WorkflowAprovacaoNivelNotificacaoMap()
        {
            ToTable("wf_aprovacao_nivel_notificacao");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
            Map(c => c.WorkflowAprovacaoNivelId).ToColumn("wf_aprovacao_nivel_id");
        }
    }
}
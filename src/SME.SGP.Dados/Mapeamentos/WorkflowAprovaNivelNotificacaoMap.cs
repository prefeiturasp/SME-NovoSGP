using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovaNivelNotificacaoMap : DommelEntityMap<WorkflowAprovaNivelNotificacao>
    {
        public WorkflowAprovaNivelNotificacaoMap()
        {
            ToTable("wf_aprova_nivel_notificacao");
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
            Map(c => c.WorkflowAprovaNivelId).ToColumn("wf_aprova_nivel_id");
        }
    }
}
using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovaNiveisNotificacaoMap : DommelEntityMap<WorkflowAprovaNiveisNotificacao>
    {
        public WorkflowAprovaNiveisNotificacaoMap()
        {
            ToTable("wf_aprova_niveis_notificacao");
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
            Map(c => c.WorkflowAprovaNiveisId).ToColumn("wf_aprova_niveis_id");
        }
    }
}
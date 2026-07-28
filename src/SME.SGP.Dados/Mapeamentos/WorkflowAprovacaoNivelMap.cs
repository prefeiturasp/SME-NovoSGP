using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovacaoNivelMap : BaseEntityMap<WorkflowAprovacaoNivel>
    {
        public WorkflowAprovacaoNivelMap()
        {
            ToTable("wf_aprovacao_nivel");
            Map(nameof(WorkflowAprovacaoNivel.Cargo), "cargo");
            Map(nameof(WorkflowAprovacaoNivel.Nivel), "nivel");
            Map(nameof(WorkflowAprovacaoNivel.Observacao), "observacao");
            Map(nameof(WorkflowAprovacaoNivel.Status), "status");
            Map(nameof(WorkflowAprovacaoNivel.WorkflowId), "wf_aprovacao_id");
        }
    }
}
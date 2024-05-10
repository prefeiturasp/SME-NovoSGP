using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovacaoNivelMap : BaseMap<WorkflowAprovacaoNivel>
    {
        public WorkflowAprovacaoNivelMap()
        {
            ToTable("wf_aprovacao_nivel");
            Map(c => c.Workflow).Ignore();
            Map(c => c.Cargo).ToColumn("cargo");
            Map(c => c.Nivel).ToColumn("nivel");
            Map(c => c.Observacao).ToColumn("observacao");
            Map(c => c.Status).ToColumn("status");
            Map(c => c.WorkflowId).ToColumn("wf_aprovacao_id");
        }
    }
}
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovacaoNivelMap : BaseMap<WorkflowAprovacaoNivel>
    {
        public WorkflowAprovacaoNivelMap()
        {
            ToTable("wf_aprovacao_nivel");
            Map(c => c.Workflow).Ignore();
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.Status).ToColumn("status");

            //Map(c => c.Workflow.Id).ToColumn("");
        }
    }
}
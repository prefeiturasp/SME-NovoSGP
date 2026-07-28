using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovacaoNivelUsuarioMap : SimpleEntityMap<WorkflowAprovacaoNivelUsuario>
    {
        public WorkflowAprovacaoNivelUsuarioMap()
        {
            ToTable("wf_aprovacao_nivel_usuario");
            Map(nameof(WorkflowAprovacaoNivelUsuario.UsuarioId), "usuario_id");
            Map(nameof(WorkflowAprovacaoNivelUsuario.WorkflowAprovacaoNivelId), "wf_aprovacao_nivel_id");
        }
    }
}
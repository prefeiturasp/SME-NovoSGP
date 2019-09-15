using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovacaoNivelUsuarioMap : DommelEntityMap<WorkflowAprovacaoNivelUsuario>
    {
        public WorkflowAprovacaoNivelUsuarioMap()
        {
            ToTable("wf_aprovacao_nivel_usuario");

            Map(c => c.Usuario).Ignore();
            Map(c => c.WorkflowAprovacaoNivel).Ignore();

            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.WorkflowAprovacaoNivelId).ToColumn("wf_aprovacao_nivel_id");
        }
    }
}
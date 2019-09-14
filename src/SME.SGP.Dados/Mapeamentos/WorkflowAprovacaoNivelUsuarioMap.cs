using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovacaoNivelUsuarioMap : DommelEntityMap<WorkflowAprovacaoNivelUsuario>
    {
        public WorkflowAprovacaoNivelUsuarioMap()
        {
            ToTable("wf_aprova_nivel_usuario");
            Map(c => c.Usuario).Ignore();
            Map(c => c.WorkflowAprovacaoNivel).Ignore();

            Map(c => c.UsuarioId).ToColumn("Usuario_id");
            Map(c => c.WorkflowAprovacaoNivelId).ToColumn("wf_aprova_nivel_id").IsKey();
        }
    }
}
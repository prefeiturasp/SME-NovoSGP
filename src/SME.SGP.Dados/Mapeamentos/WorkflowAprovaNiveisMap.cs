using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovaNiveisMap : BaseMap<WorkflowAprovaNiveis>
    {
        public WorkflowAprovaNiveisMap()
        {
            ToTable("wf_aprova_niveis");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
        }
    }
}
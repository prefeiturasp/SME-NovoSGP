using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovaNiveisMap : BaseMap<WorkflowAprovaNiveis>
    {
        public WorkflowAprovaNiveisMap()
        {
            ToTable("wf_aprova_niveis");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.EscolaId).ToColumn("escola_id");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}
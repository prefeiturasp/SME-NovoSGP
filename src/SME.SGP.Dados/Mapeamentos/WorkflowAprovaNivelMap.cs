using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovaNivelMap : BaseMap<WorkflowAprovaNivel>
    {
        public WorkflowAprovaNivelMap()
        {
            ToTable("wf_aprova_nivel");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.EscolaId).ToColumn("escola_id");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}
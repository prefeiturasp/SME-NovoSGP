using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class DiarioBordoMap: BaseMap<DiarioBordo>
    {
        public DiarioBordoMap()
        {
            ToTable("diario_bordo");
            Map(a => a.AulaId).ToColumn("aula_id");
            Map(a => a.DevolutivaId).ToColumn("devolutiva_id");
            Map(a => a.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(a => a.TurmaId).ToColumn("turma_id");
            Map(a => a.InseridoCJ).ToColumn("inserido_cj");
            Map(a => a.Excluido).ToColumn("excluido");
            Map(a => a.Migrado).ToColumn("migrado");
            Map(c => c.Planejamento).ToColumn("planejamento");
        }
    }
}

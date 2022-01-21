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
            Map(a => a.ReflexoesReplanejamento).ToColumn("reflexoes_replanejamento");
            Map(a => a.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(a => a.TurmaId).ToColumn("turma_id");
        }
    }
}

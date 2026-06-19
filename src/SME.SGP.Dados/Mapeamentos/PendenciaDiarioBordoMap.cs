using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PendenciaDiarioBordoMap : BaseMap<PendenciaDiarioBordo>
    {
        public PendenciaDiarioBordoMap()
        {
            ToTable("pendencia_diario_bordo");
            Map(c => c.AulaId).ToColumn("aula_id");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
            Map(c => c.ComponenteId).ToColumn("componente_curricular_id");
            Map(c => c.ProfessorRf).ToColumn("professor_rf");
        }
    }
}

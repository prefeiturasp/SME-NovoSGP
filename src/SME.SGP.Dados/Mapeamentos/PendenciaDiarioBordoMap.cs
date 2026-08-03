using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class PendenciaDiarioBordoMap : BaseMap<PendenciaDiarioBordo>
    {
        public PendenciaDiarioBordoMap()
        {
            ToTable("pendencia_diario_bordo");
            Map(nameof(PendenciaDiarioBordo.AulaId), "aula_id");
            Map(nameof(PendenciaDiarioBordo.PendenciaId), "pendencia_id");
            Map(nameof(PendenciaDiarioBordo.ComponenteId), "componente_curricular_id");
            Map(nameof(PendenciaDiarioBordo.ProfessorRf), "professor_rf");
        }
    }
}
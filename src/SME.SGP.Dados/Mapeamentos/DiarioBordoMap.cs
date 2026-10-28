using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class DiarioBordoMap : BaseMap<DiarioBordo>
    {
        public DiarioBordoMap()
        {
            ToTable("diario_bordo");
            Map(nameof(DiarioBordo.AulaId), "aula_id");
            Map(nameof(DiarioBordo.DevolutivaId), "devolutiva_id");
            Map(nameof(DiarioBordo.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(DiarioBordo.TurmaId), "turma_id");
            Map(nameof(DiarioBordo.InseridoCJ), "inserido_cj");
            Map(nameof(DiarioBordo.Excluido), "excluido");
            Map(nameof(DiarioBordo.Migrado), "migrado");
            Map(nameof(DiarioBordo.Planejamento), "planejamento");
        }
    }
}
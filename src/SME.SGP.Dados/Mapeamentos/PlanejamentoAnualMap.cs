using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanejamentoAnualMap : SimpleMap<PlanejamentoAnual>
    {
        public PlanejamentoAnualMap()
        {
            ToTable("planejamento_anual");
            Map(nameof(PlanejamentoAnual.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(PlanejamentoAnual.Migrado), "migrado");
            Map(nameof(PlanejamentoAnual.TurmaId), "turma_id");
        }
    }
}
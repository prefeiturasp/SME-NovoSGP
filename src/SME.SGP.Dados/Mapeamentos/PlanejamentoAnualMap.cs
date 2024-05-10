using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanejamentoAnualMap : BaseMap<PlanejamentoAnual>
    {
        public PlanejamentoAnualMap()
        {
            ToTable("planejamento_anual");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}
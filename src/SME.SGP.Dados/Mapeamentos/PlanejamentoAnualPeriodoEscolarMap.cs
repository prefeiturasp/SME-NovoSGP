using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanejamentoAnualPeriodoEscolarMap : BaseMap<PlanejamentoAnualPeriodoEscolar>
    {
        public PlanejamentoAnualPeriodoEscolarMap()
        {
            ToTable("planejamento_anual_periodo_escolar");
            Map(c => c.PlanejamentoAnualId).ToColumn("planejamento_anual_id");
            Map(c => c.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
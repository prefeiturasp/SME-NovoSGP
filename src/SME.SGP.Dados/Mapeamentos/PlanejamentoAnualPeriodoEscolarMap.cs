using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanejamentoAnualPeriodoEscolarMap : BaseEntityMap<PlanejamentoAnualPeriodoEscolar>
    {
        public PlanejamentoAnualPeriodoEscolarMap()
        {
            ToTable("planejamento_anual_periodo_escolar");
            Map(nameof(PlanejamentoAnualPeriodoEscolar.PeriodoEscolarId), "periodo_escolar_id");
            Map(nameof(PlanejamentoAnualPeriodoEscolar.PlanejamentoAnualId), "planejamento_anual_id");
            Map(nameof(PlanejamentoAnualPeriodoEscolar.Excluido), "excluido");
        }
    }
}
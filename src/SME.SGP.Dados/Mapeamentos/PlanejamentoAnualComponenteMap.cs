using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanejamentoAnualComponenteMap : SimpleMap<PlanejamentoAnualComponente>
    {
        public PlanejamentoAnualComponenteMap()
        {
            ToTable("planejamento_anual_componente");
            Map(nameof(PlanejamentoAnualComponente.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(PlanejamentoAnualComponente.Descricao), "descricao");
            Map(nameof(PlanejamentoAnualComponente.PlanejamentoAnualPeriodoEscolarId), "planejamento_anual_periodo_escolar_id");
            Map(nameof(PlanejamentoAnualComponente.Excluido), "excluido");
        }
    }
}
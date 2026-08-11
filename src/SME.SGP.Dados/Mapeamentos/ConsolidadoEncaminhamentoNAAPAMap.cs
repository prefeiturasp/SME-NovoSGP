using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidadoEncaminhamentoNAAPAMap : BaseMap<ConsolidadoEncaminhamentoNAAPA>
    {
        public ConsolidadoEncaminhamentoNAAPAMap()
        {
            ToTable("consolidado_encaminhamento_naapa");
            Map(nameof(ConsolidadoEncaminhamentoNAAPA.AnoLetivo), "ano_letivo");
            Map(nameof(ConsolidadoEncaminhamentoNAAPA.UeId), "ue_id");
            Map(nameof(ConsolidadoEncaminhamentoNAAPA.Quantidade), "quantidade");
            Map(nameof(ConsolidadoEncaminhamentoNAAPA.Situacao), "situacao");
            Map(nameof(ConsolidadoEncaminhamentoNAAPA.Modalidade), "modalidade_codigo");
        }
    }
}
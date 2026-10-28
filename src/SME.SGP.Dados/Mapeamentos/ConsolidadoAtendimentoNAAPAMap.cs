using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidadoAtendimentoNAAPAMap : BaseMap<ConsolidadoAtendimentoNAAPA>
    {
        public ConsolidadoAtendimentoNAAPAMap()
        {
            ToTable("consolidado_atendimento_naapa");
            Map(nameof(ConsolidadoAtendimentoNAAPA.AnoLetivo), "ano_letivo");
            Map(nameof(ConsolidadoAtendimentoNAAPA.Mes), "mes");
            Map(nameof(ConsolidadoAtendimentoNAAPA.NomeProfissional), "nome_profissional");
            Map(nameof(ConsolidadoAtendimentoNAAPA.RfProfissional), "rf_profissional");
            Map(nameof(ConsolidadoAtendimentoNAAPA.UeId), "ue_id");
            Map(nameof(ConsolidadoAtendimentoNAAPA.Quantidade), "quantidade");
            Map(nameof(ConsolidadoAtendimentoNAAPA.Modalidade), "modalidade_codigo");
        }
    }
}
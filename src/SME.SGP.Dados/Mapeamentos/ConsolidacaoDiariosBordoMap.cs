using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ConsolidacaoDiariosBordoMap : SimpleEntityMap<ConsolidacaoDiariosBordo>
    {
        public ConsolidacaoDiariosBordoMap()
        {
            ToTable("consolidacao_diarios_bordo");
            Map(nameof(ConsolidacaoDiariosBordo.TurmaId), "turma_id");
            Map(nameof(ConsolidacaoDiariosBordo.AnoLetivo), "ano_letivo");
            Map(nameof(ConsolidacaoDiariosBordo.QuantidadePreenchidos), "quantidade_preenchidos");
            Map(nameof(ConsolidacaoDiariosBordo.QuantidadePendentes), "quantidade_pendentes");
        }
    }
}
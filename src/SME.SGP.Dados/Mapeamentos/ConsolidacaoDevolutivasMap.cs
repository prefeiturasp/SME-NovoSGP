
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoDevolutivasMap : SimpleEntityMap<ConsolidacaoDevolutivas>
    {
        public ConsolidacaoDevolutivasMap()
        {
            ToTable("consolidacao_devolutivas");
            Map(nameof(ConsolidacaoDevolutivas.TurmaId),"turma_id");
            Map(nameof(ConsolidacaoDevolutivas.QuantidadeEstimadaDevolutivas),"quantidade_estimada_devolutivas");
            Map(nameof(ConsolidacaoDevolutivas.QuantidadeRegistradaDevolutivas),"quantidade_registrada_devolutivas");
        }
    }
}
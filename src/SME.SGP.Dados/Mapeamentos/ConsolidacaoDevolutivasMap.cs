using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoDevolutivasMap : DommelEntityMap<ConsolidacaoDevolutivas>
    {
        public ConsolidacaoDevolutivasMap()
        {
            ToTable("consolidacao_devolutivas");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.QuantidadeEstimadaDevolutivas).ToColumn("quantidade_estimada_devolutivas");
            Map(c => c.QuantidadeRegistradaDevolutivas).ToColumn("quantidade_registrada_devolutivas");
            Map(c => c.QuantidadeComDevolutiva).ToColumn("quantidade_com_devolutiva");
            Map(c => c.QuantidadeComDevolutivaPendente).ToColumn("quantidade_com_devolutiva_pendente");
            Map(c => c.QuantidadeDiariosDeBordoComReflexoesEPlanejamentosPrenchidos).ToColumn("quantidade_com_reflexoes_planejamentos_preenchidos");
            Map(c => c.QuantidadeDiariosDeBordoSemReflexoesEPlanejamentosPrenchidos).ToColumn("quantidade_sem_reflexoes_planejamentos_preenchidos");
        }
    }
}
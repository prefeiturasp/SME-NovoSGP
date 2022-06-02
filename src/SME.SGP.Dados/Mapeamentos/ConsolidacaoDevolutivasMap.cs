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
            Map(c => c.QuantidadeEstimadaDevolutivas).ToColumn("quantidade_estimada_devolutivas");
            Map(c => c.QuantidadeRegistradaDevolutivas).ToColumn("quantidade_registrada_devolutivas");
        }
    }
}
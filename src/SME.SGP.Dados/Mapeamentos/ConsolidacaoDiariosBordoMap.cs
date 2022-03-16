using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ConsolidacaoDiariosBordoMap : DommelEntityMap<ConsolidacaoDiariosBordo>
    {
        public ConsolidacaoDiariosBordoMap()
        {
            ToTable("consolidacao_diarios_bordo");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.QuantidadePreenchidos).ToColumn("quantidade_preenchidos");
            Map(c => c.QuantidadePendentes).ToColumn("quantidade_pendentes");

        }
    }
}

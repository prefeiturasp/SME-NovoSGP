using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ConsolidacaoDiariosBordoMap : DommelEntityMap<ConsolidacaoDiariosBordo>
    {
        public ConsolidacaoDiariosBordoMap()
        {
            ToTable("consolidacao_diarios_bordo");
            Map(a => a.TurmaId).ToColumn("turma_id");
            Map(a => a.AnoLetivo).ToColumn("ano_letivo");
            Map(a => a.QuantidadePreenchidos).ToColumn("quantidade_preenchidos");
            Map(a => a.QuantidadePendentes).ToColumn("quantidade_pendentes");
        }
    }
}

using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidadoEncaminhamentoNAAPAMap : BaseMap<ConsolidadoEncaminhamentoNAAPA>
    {
        public ConsolidadoEncaminhamentoNAAPAMap()
        {
            ToTable("consolidado_encaminhamento_naapa");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.Quantidade).ToColumn("quantidade");
            Map(c => c.Situacao).ToColumn("situacao");
        }
    }
}
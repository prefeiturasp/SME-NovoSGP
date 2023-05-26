using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidadoEncaminhamentoNAAPAMap : DommelEntityMap<ConsolidadoEncaminhamentoNAAPA>
    {
        public ConsolidadoEncaminhamentoNAAPAMap()
        {
            ToTable("consolidado_encaminhamento_naapa");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.Quantidade).ToColumn("quantidade");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.AlteradoEm).ToColumn("alterado_em");
            Map(c => c.AlteradoPor).ToColumn("alterado_por");
            Map(c => c.AlteradoRF).ToColumn("alterado_rf");
            Map(c => c.CriadoRF).ToColumn("criado_rf");
            Map(c => c.CriadoPor).ToColumn("criado_por");
            Map(c => c.CriadoEm).ToColumn("criado_em");
        }
    }
}
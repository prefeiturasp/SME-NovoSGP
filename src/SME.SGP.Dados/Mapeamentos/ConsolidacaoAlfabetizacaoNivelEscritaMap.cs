using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoAlfabetizacaoNivelEscritaMap : DommelEntityMap<ConsolidacaoAlfabetizacaoNivelEscrita>
    {
        public ConsolidacaoAlfabetizacaoNivelEscritaMap()
        {
            ToTable("consolidacao_alfabetizacao_nivel_escrita");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.DreCodigo).ToColumn("dre_codigo");
            Map(c => c.UeCodigo).ToColumn("ue_codigo");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Periodo).ToColumn("periodo");
            Map(c => c.NivelEscrita).ToColumn("nivel_escrita");
            Map(c => c.Quantidade).ToColumn("quantidade");
        }
    }
}
using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DreMap : DommelEntityMap<Dre>
    {
        public DreMap()
        {
            ToTable("dre");
            Map(c => c.Abreviacao).ToColumn("abreviacao");
            Map(c => c.CodigoDre).ToColumn("dre_id");
            Map(c => c.DataAtualizacao).ToColumn("data_atualizacao");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.PrefixoDoNomeAbreviado).Ignore();
        }
    }
}
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DreMap : SimpleEntityMap<Dre>
    {
        public DreMap()
        {
            ToTable("dre");
            Map(nameof(Dre.Abreviacao), "abreviacao");
            Map(nameof(Dre.CodigoDre), "dre_id");
            Map(nameof(Dre.DataAtualizacao), "data_atualizacao");
            Map(nameof(Dre.Nome), "nome");
            Ignore(nameof(Dre.PrefixoDoNomeAbreviado));
        }
    }
}
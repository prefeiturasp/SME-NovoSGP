using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DreMap : SimpleMap<Dre>
    {
        public DreMap()
        {
            ToTable("dre");
            Map(nameof(Dre.Abreviacao), "abreviacao");
            Map(nameof(Dre.CodigoDre), "dre_id");
            Map(nameof(Dre.DataAtualizacao), "data_atualizacao");
            Map(nameof(Dre.Nome), "nome");
        }
    }
}
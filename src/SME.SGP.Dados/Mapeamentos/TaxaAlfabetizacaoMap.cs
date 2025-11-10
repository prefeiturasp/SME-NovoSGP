using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TaxaAlfabetizacaoMap : BaseMap<TaxaAlfabetizacao>
    {
        public TaxaAlfabetizacaoMap()
        {
            ToTable("taxa_alfabetizacao");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.CodigoEOLEscola).ToColumn("codigo_eol_escola");
            Map(c => c.Taxa).ToColumn("taxa");
        }
    }
}

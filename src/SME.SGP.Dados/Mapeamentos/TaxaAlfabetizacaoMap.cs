using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TaxaAlfabetizacaoMap : BaseEntityMap<TaxaAlfabetizacao>
    {
        public TaxaAlfabetizacaoMap()
        {
            ToTable("taxa_alfabetizacao");

            Map(nameof(TaxaAlfabetizacao.AnoLetivo), "ano_letivo");
            Map(nameof(TaxaAlfabetizacao.CodigoEOLEscola), "codigo_eol_escola");
            Map(nameof(TaxaAlfabetizacao.Taxa), "taxa");
        }
    }
}
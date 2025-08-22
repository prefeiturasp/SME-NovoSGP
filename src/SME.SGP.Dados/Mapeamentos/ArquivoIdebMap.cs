using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ArquivoIdebMap : BaseMap<ArquivoIdeb>
    {
        public ArquivoIdebMap()
        {
            ToTable("arquivo_ideb");
            Map(c => c.SerieAno).ToColumn("serie_ano");
            Map(c => c.CodigoEOLEscola).ToColumn("codigo_eol_escola");
            Map(c => c.Nota).ToColumn("nota");
        }
    }
}

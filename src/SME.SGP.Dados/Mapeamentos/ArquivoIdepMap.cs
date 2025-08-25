using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ArquivoIdepMap : BaseMap<ArquivoIdep>
    {
        public ArquivoIdepMap()
        {
            ToTable("arquivo_idep");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.SerieAno).ToColumn("serie_ano");
            Map(c => c.CodigoEOLEscola).ToColumn("codigo_eol_escola");
            Map(c => c.Nota).ToColumn("nota");
        }
    }
}

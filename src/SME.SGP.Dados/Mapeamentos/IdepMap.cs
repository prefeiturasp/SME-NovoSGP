using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class IdepMap : BaseMap<Idep>
    {
        public IdepMap()
        {
            ToTable("idep");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.SerieAno).ToColumn("serie_ano");
            Map(c => c.CodigoEOLEscola).ToColumn("codigo_eol_escola");
            Map(c => c.Nota).ToColumn("nota");
        }
    }
}

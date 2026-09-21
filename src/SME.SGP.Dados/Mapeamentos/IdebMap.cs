using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class IdebMap : BaseMap<Ideb>
    {
        public IdebMap()
        {
            ToTable("ideb");
            Map(nameof(Ideb.AnoLetivo), "ano_letivo");
            Map(nameof(Ideb.SerieAno), "serie_ano");
            Map(nameof(Ideb.CodigoEOLEscola), "codigo_eol_escola");
            Map(nameof(Ideb.Nota), "nota");
        }
    }
}
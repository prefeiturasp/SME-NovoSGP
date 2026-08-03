using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class IdepMap : BaseMap<Idep>
    {
        public IdepMap()
        {
            ToTable("idep");
            Map(nameof(Idep.AnoLetivo), "ano_letivo");
            Map(nameof(Idep.SerieAno), "serie_ano");
            Map(nameof(Idep.CodigoEOLEscola), "codigo_eol_escola");
            Map(nameof(Idep.Nota), "nota");
        }
    }
}
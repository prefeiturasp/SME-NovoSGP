using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ProficienciaIdebMap : BaseMap<ProficienciaIdeb>
    {
        public ProficienciaIdebMap()
        {
            ToTable("proficiencia_ideb");
            Map(nameof(ProficienciaIdeb.CodigoUe), "codigo_eol_escola");
            Map(nameof(ProficienciaIdeb.SerieAno), "serie_ano");
            Map(nameof(ProficienciaIdeb.ComponenteCurricular), "componente_curricular");
            Map(nameof(ProficienciaIdeb.Proficiencia), "proficiencia");
            Map(nameof(ProficienciaIdeb.AnoLetivo), "ano_letivo");
            Map(nameof(ProficienciaIdeb.Boletim), "boletim");
        }
    }
}
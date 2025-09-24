using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ProficienciaIdebMap : BaseMap<ProficienciaIdeb>
    {
        public ProficienciaIdebMap()
        {
            ToTable("proficiencia_ideb");
            Map(c => c.CodigoEOLEscola).ToColumn("codigo_eol_escola");
            Map(c => c.SerieAno).ToColumn("serie_ano");
            Map(c => c.ComponenteCurricular).ToColumn("componente_curricular");
            Map(c => c.Proficiencia).ToColumn("proficiencia");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Boletim).ToColumn("boletim");
        }
    }
}

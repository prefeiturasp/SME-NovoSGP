using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ProficienciaIdepMap : BaseMap<ProficienciaIdep>
    {
        public ProficienciaIdepMap()
        {
            ToTable("proficiencia_idep");
            Map(nameof(ProficienciaIdep.CodigoUe), "codigo_eol_escola");
            Map(nameof(ProficienciaIdep.SerieAno), "serie_ano");
            Map(nameof(ProficienciaIdep.ComponenteCurricular), "componente_curricular");
            Map(nameof(ProficienciaIdep.Proficiencia), "proficiencia");
            Map(nameof(ProficienciaIdep.AnoLetivo), "ano_letivo");
            Map(nameof(ProficienciaIdep.Boletim), "boletim");
        }
    }
}
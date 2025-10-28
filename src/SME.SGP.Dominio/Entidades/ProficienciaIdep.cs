using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Dominio.Entidades
{
    public class ProficienciaIdep: EntidadeBase
    {
        public string CodigoUe { get; set; }
        public SerieAnoIndiceDesenvolvimentoEnum SerieAno { get; set; }
        public string ComponenteCurricular { get; set; }
        public decimal Proficiencia { get; set; }
        public int AnoLetivo { get; set; }
        public string Boletim { get; set; }
    }
}
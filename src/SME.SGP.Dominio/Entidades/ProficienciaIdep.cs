namespace SME.SGP.Dominio
{
    public class ProficienciaIdep: EntidadeBase
    {
        public string CodigoEOLEscola { get; set; }
        public int SerieAno { get; set; }
        public string ComponenteCurricular { get; set; }
        public decimal Proficiencia { get; set; }
        public int AnoLetivo { get; set; }
        public string Boletim { get; set; }
    }
}

namespace SME.SGP.Dominio
{
    public class ProficienciaIdep: EntidadeBase
    {
        public string CodigoEOLEscola { get; set; }
        public long SerieAno { get; set; }
        public int ComponenteCurricular { get; set; }
        public decimal Proficiencia { get; set; }
        public long AnoLetivo { get; set; }
        public string Boletim { get; set; }
    }
}

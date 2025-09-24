namespace SME.SGP.Dominio.Entidades
{
    public class ProficienciaIdeb : EntidadeBase
    {
        public string CodigoEOLEscola { get; set; }
        public int SerieAno { get; set; }
        public int ComponenteCurricular { get; set; }
        public decimal Proficiencia { get; set; }
        public int AnoLetivo { get; set; }
        public string Boletim { get; set; }
    }
}

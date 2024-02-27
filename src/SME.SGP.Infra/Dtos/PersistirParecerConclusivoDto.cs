namespace SME.SGP.Dto
{
    public class PersistirParecerConclusivoDto
    {
        public long ConselhoClasseAlunoId { get; set; }
        public long? ParecerConclusivoId { get; set; }
        public string ConselhoClasseAlunoCodigo { get; set; }
        public long TurmaId { get; set; }
        public int? Bimestre { get; set; }
        public int AnoLetivo { get; set; }
        public string TurmaCodigo { get; set; }
    }
}
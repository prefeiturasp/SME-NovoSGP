namespace SME.SGP.Infra
{
    public class NotaConceitoComponenteBimestreAlunoDto
    {
        public long ConselhoClasseId { get; set; }
        public long ConselhoClasseNotaId { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public long? ConceitoId { get; set; }
        public double? Nota { get; set; }
        public int? Bimestre { get; set; }
        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }
    }
}
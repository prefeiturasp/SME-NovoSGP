namespace SME.SGP.Infra
{
    public class NotaConceitoBimestreComponenteDto
    {
        public long Id { get; set; }
        public long ConselhoClasseNotaId { get; set; }
        public int? Bimestre { get; set; }
        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public long? ConceitoId { get; set; }
        public double? Nota { get; set; }
        public double? NotaConceito => ConceitoId ?? Nota;
    }
}

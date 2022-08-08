namespace SME.SGP.Infra
{
    public class CacheNotaConceitoComponenteDto
    {
        public long Id { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public long ConselhoClasseNotaId { get; set; }        
        public string AlunoCodigo { get; set; }
        public long? ConceitoId { get; set; }
        public double? Nota { get; set; }
        public double? NotaConceito => ConceitoId ?? Nota;
    }
}
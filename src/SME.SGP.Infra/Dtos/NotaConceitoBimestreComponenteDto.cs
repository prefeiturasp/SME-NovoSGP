using System;

namespace SME.SGP.Infra
{
    public class NotaConceitoBimestreComponenteDto
    {
        public long ConselhoClasseId { get; set; }
        public long ConselhoClasseNotaId { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public string ComponenteCurricularNome { get; set; }
        public long? ConceitoId { get; set; }
        public double? Nota { get; set; }
        public int? Bimestre { get; set; }
        public string AlunoCodigo { get; set; }
        public double? NotaConceito => ConceitoId ?? Nota;
        public string Conceito { get; set; }
        public string NotaConceitoFormatado { get => ConceitoId.HasValue ? Conceito : String.Format("{0:0.0}", Nota); }
        public string TurmaCodigo { get; set; }
    }
}

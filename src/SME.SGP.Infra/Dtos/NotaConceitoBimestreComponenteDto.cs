using System;

namespace SME.SGP.Infra
{
    public class NotaConceitoBimestreComponenteDto
    {
        public long Id { get; set; }
        public long ConselhoClasseNotaId { get; set; }
        public int? Bimestre { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public string ComponenteCurricularNome { get; set; }
        public long? ConceitoId { get; set; }
        public double Nota { get; set; }
        public string Conceito { get; set; }

        public string NotaConceitoFormatado { get => ConceitoId.HasValue ? Conceito : String.Format("{0:0.0}", Nota); }

        public double NotaConceito { get => ConceitoId.HasValue ? ConceitoId.Value : Nota; }
    }
}

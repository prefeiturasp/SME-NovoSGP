using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class NotaConceitoBimestreComponenteDto
    {
        public long Id { get; set; }
        public long ConselhoClasseNotaId { get; set; }
        public int? Bimestre { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public long? ConceitoId { get; set; }
        public double Nota { get; set; }

        public double NotaConceito { get => ConceitoId.HasValue ? ConceitoId.Value : Nota; }
    }
}

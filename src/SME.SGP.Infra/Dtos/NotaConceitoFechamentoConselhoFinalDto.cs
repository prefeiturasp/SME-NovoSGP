using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class NotaConceitoFechamentoConselhoFinalDto
    {
        public long ConselhoClasseAlunoId { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public long? ConceitoId { get; set; }
        public double Nota { get; set; }

        public double NotaConceito { get => ConceitoId.HasValue ? ConceitoId.Value : Nota; }
    }
}

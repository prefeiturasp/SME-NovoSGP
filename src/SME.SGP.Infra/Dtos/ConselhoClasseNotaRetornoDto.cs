using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ConselhoClasseNotaRetornoDto
    {
        public long ConselhoClasseId { get; set; }
        public long FechamentoTurmaId { get; set; }
        public AuditoriaDto Auditoria { get; set; }

        public long ConselhoClasseAlunoId { get; set; }
        public bool EmAprovacao { get; set; }
    }
}

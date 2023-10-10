using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AuditoriaRelatorioSemestralAlunoDto
    {
        public long RelatorioSemestralId { get; set; }
        public long RelatorioSemestralAlunoId { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}

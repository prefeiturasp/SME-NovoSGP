using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RelatorioSemestralAlunoDto
    {        
        public long RelatorioSemestralId { get; set; }
        public long RelatorioSemestralAlunoId { get; set; }
        public IEnumerable<RelatorioSemestralAlunoSecaoDto> Secoes { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }

}

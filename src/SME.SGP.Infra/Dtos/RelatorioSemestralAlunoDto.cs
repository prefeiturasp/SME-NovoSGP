using System.Collections.Generic;

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

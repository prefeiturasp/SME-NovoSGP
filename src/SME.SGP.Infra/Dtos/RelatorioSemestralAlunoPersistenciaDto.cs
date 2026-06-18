using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RelatorioSemestralAlunoPersistenciaDto
    {
        public long RelatorioSemestralId { get; set; }
        public long RelatorioSemestralAlunoId { get; set; }
        public IEnumerable<RelatorioSemestralAlunoSecaoDto> Secoes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RelatorioSemestralAlunoPersistenciaDto
    {
        public IEnumerable<RelatorioSemestralAlunoSecaoDto> Secoes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaAlunoAulaDto
    {
        public long Id { get; set; }
        public long CompensacaoAusenciaAlunoId { get; set; }
        public long RegistroFrequenciaAlunoId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FrequenciaAlunoAulaDto
    {
        public int NumeroAula { get; set; }
        public int TipoFrequencia { get; set; }
        public string AlunoCodigo { get; set; }
        public long FrequenciaAlunoId { get; set; }
    }
}

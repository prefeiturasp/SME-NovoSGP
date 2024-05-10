using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AulaRecorrenciaDto
    {
        public long AulaId { get; set; }
        public int RecorrenciaAula { get; set; }
        public int QuantidadeAulasRecorrentes { get; set; }
        public bool ExisteFrequenciaOuPlanoAula { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlunoAusenteDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public double PercentualFrequencia { get; set; }
        public int QuantidadeFaltas { get; set; }
        public int MaximoCompensacoesPermitidas { get; set; }
        public bool Alerta { get; set; }
    }
}

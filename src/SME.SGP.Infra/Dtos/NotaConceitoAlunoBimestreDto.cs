using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class NotaConceitoAlunoBimestreDto
    {
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public IEnumerable<NotaConceitoBimestreRetornoDto> Notas { get; set; }
        public int QuantidadeFaltas { get; set; }
        public int QuantidadeCompensacoes { get; set; }
        public double PercentualFrequencia { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class NotaConceitoAlunoBimestreDto
    {
        public int NumeroChamada { get; set; }
        public string Nome { get; set; }
        public string Informacao { get; set; }
        public bool Ativo { get; set; }
        public IEnumerable<NotaConceitoBimestreRetornoDto> Notas { get; set; }
        public double QuantidadeFaltas { get; set; }
        public double QuantidadeCompensacoes { get; set; }
        public double PercentualFrequencia { get; set; }
    }
}

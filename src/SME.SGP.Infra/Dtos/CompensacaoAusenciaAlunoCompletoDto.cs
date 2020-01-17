using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaAlunoCompletoDto
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public double QuantidadeFaltasTotais { get; set; }
        public double QuantidadeFaltasCompensadas { get; set; }
        public double PercentualFrequencia { get; set; }
    }
}

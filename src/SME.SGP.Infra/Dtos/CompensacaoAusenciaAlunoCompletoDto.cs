using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaAlunoCompletoDto
    {
        public string Id { get; set; }
        public string AlunoNome { get; set; }
        public double QtdFaltasCompensadas { get; set; }
        public double QuantidadeFaltasTotais { get; set; }
    }
}

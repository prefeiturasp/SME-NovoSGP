using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlunoFaltosoBimestreDto
    {
        public string DreCodigo { get; set; }
        public string DreNome { get; set; }
        public int TipoEscola { get; set; }
        public string UeCodigo { get; set; }
        public string UeNome { get; set; }
        public string TurmaCodigo { get; set; }
        public string TurmaNome { get; set; }
        public string AlunoCodigo { get; set; }
        public double PercentualFaltas { get; set; }
    }
}

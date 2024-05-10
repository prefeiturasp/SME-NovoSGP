using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaAlunoQtdDto
    {
        public int NumeroAluno { get; set; }
        public string CodigoAluno { get; set; }
        public string NomeAluno { get; set; }
        public int QuantidadeCompensacoes { get; set; }
        public double PercentualFrequencia { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPesquisaFuncionarioDto
    {
        public string CodigoRF { get; set; }
        public string CodigoDRE { get; set; }
        public string CodigoUE { get; set; }
        public string CodigoTurma { get; set; }
        public string Nome { get; set; }
        public int Limite { get; set; }
    }
}

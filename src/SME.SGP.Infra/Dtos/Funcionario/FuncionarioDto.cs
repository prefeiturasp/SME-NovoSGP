using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FuncionarioDto
    {
        public int CodigoRF { get; set; }
        public string NomeServidor { get; set; }

        public string DataInicio { get; set; }

        public string DataFim { get; set; }

        public string Cargo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ArquivoDto
    {
        public Guid Codigo { get; set; }
        public string Nome { get; set; }
        public (byte[], string, string) Download { get; set; }
    }
}

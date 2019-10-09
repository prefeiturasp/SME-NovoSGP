using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class TipoCalendarioEscolar: EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public string Nome { get; set; }
        public Periodo Periodo { get; set; }
        public bool Situacao { get; set; }
        public Modalidade Modalidade { get; set; }
    }
}

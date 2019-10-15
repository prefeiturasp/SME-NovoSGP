using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class TipoCalendario: EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public string Nome { get; set; }
        public Periodo Periodo { get; set; }
        public bool Situacao { get; set; }
        public ModalidadeTipoCalendario Modalidade { get; set; }
        public bool Excluido { get; set; }
    }
}

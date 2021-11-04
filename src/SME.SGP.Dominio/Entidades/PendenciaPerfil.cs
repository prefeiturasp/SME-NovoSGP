using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class PendenciaPerfil : EntidadeBase
    {
        public int Cargo { get; set; }
        public int Nivel { get; set; }
        public long PendenciaId { get; set; }
        public Pendencia Pendencia { get; set; }       
    }
}

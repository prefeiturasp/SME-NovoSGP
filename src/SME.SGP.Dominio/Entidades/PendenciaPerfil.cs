using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class PendenciaPerfil : EntidadeBase
    {
        public int PerfilCodigo { get; set; }
        public long PendenciaId { get; set; }
        public Pendencia Pendencia { get; set; }       
    }
}

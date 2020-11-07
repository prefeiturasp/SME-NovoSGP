using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class PendenciaParametroEvento : EntidadeBase
    {
        public long PendenciaId { get; set; }
        public Pendencia Pendencia { get; set; }

        public long ParametroSistemaId { get; set; }
    }
}

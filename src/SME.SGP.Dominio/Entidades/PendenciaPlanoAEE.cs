using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class PendenciaPlanoAEE : EntidadeBase
    {
        public PlanoAEE PlanoAEE { get; set; }
        public long PlanoAEEId { get; set; }

        public Pendencia Pendencia { get; set; }
        public long PendenciaId { get; set; }
    }
}

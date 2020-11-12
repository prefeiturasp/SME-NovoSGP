using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class PendenciaParametroEvento : EntidadeBase
    {
        public long PendenciaCalendarioUeId { get; set; }
        public PendenciaCalendarioUe PendenciaCalendarioUe { get; set; }
        public long ParametroSistemaId { get; set; }
        public int QuantidadeEventos { get; set; }
    }
}

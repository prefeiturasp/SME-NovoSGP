using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FrequenciaDetalhadaPorDataDto
    {
        public DateTime DataAula { get; set; }
        public long AulaId { get; set; }
        public long NumeroAula { get; set; }
        public long TipoCalendario { get; set; }
    }
}

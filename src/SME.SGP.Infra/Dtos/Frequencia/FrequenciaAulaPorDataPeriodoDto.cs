using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FrequenciaAulaPorDataPeriodoDto
    {
        public DateTime DataAula { get; set; }
        public string TipoFrequencia { get; set; }
        public long AulaId { get; set; }
    }
}

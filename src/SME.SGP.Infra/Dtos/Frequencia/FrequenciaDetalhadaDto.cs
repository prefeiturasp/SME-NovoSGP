using System;

namespace SME.SGP.Infra
{
    public class FrequenciaDetalhadaDto
    {
        public DateTime DataAula { get; set; }
        public long NumeroAula { get; set; }
        public IndicativoFrequenciaDto IndicativoFrequencia { get; set; }
    }
}

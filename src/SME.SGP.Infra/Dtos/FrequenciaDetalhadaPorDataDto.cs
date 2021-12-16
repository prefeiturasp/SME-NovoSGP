using System;

namespace SME.SGP.Infra
{
    public class FrequenciaDetalhadaPorDataDto
    {
        public DateTime DataAula { get; set; }
        public long AulaId { get; set; }
        public long TipoCalendario { get; set; }
        public string AlunoCodigo { get; set; }
    }
}

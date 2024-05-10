using System;

namespace SME.SGP.Infra.Dtos
{
    public class AulaComFrequenciaNaDataDto
    {
        public long AulaId { get; set; }
        public DateTime DataAula { get; set; }
        public long RegistroFrequenciaId { get; set; }
        public int QuantidadeAulas { get; set; }
    }
}

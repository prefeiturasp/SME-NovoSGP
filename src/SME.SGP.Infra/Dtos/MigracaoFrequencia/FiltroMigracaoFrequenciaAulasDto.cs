using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroMigracaoFrequenciaAulasDto
    {
        public string TurmaCodigo { get; set; }
        public long AulaId { get; set; }
        public long TipoCalendarioId { get; set; }
        public int QuantidadeAula { get; set; }
        public DateTime DataAula { get; set; }
        public long RegistroFrequenciaId { get; set; }
    }
}

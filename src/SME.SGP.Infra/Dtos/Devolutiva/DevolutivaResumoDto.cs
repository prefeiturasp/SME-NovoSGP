using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class DevolutivaResumoDto
    {
        public long Id { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
    }
}

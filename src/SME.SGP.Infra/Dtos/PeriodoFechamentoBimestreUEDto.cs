using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class PeriodoFechamentoBimestreUEDto
    {
        public long UeId { get; set; }
        public long PeriodoFechamentoBimestreId { get; set; }
        public DateTime InicioFechamento { get; set; }
        public DateTime FinalFechamento { get; set; }
    }
}

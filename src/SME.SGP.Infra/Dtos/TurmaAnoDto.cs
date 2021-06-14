using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class TurmaAnoDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoUE { get; set; }
    }
}

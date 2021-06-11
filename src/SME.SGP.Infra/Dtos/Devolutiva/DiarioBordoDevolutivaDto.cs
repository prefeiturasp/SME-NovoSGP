using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class DiarioBordoDevolutivaDto
    {
        public bool AulaCj { get; set; }
        public DateTime Data { get; set; }
        public string Planejamento { get; set; }
        public string PlanejamentoSimples { get; set; }
    }
}

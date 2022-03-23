using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PeriodoEscolarComponenteDto
    {
        public long Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string PeriodoEscolar { get; set; }
        public bool AulaCj { get; set; }
    }
}

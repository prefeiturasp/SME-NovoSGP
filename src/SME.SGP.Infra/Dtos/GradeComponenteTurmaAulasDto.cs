using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class GradeComponenteTurmaAulasDto
    {
        public int QuantidadeAulasGrade { get; set; }
        public int QuantidadeAulasRestante { get; set; }
        public bool PodeEditar { get; set; }
    }
}

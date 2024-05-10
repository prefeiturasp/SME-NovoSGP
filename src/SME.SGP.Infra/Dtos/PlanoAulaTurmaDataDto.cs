using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PlanoAulaTurmaDataDto
    {
        public DateTime Data { get; set; }
        public int TurmaId { get; set; }
        public string DisciplinaId { get; set; }
    }
}

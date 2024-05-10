using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class TurmaEComponenteDto
    {
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
    }
}

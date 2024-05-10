using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroAulaDto
    {
        public long UeId { get; set; }
        public long DisciplinaId { get; set; }
        public long TurmaId { get; set; }
        public long TipoCalendarioId { get; set; }
    }
}

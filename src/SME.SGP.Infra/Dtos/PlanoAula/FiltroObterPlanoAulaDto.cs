using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroObterPlanoAulaDto
    {
        public FiltroObterPlanoAulaDto(long aulaId, long turmaId)
        {
            AulaId = aulaId;
            TurmaId = turmaId;
        }

        public long AulaId { get; set; }
        public long TurmaId { get; set; }
    }
}

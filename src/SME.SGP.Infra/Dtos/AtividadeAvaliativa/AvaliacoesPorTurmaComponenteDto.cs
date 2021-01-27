using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AvaliacoesPorTurmaComponenteDto
    {
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public int QuantidadeAvaliacoes { get; set; }
    }
}

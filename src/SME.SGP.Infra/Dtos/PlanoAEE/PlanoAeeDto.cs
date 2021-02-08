using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PlanoAEEDto
    {
        public long Id { get; set; }
        public AuditoriaDto Auditoria { get; set; }
        public IEnumerable<QuestaoDto> Questoes { get; set; }
        public IEnumerable<PlanoAEEVersaoDto> Versoes { get; set; }
    }
}

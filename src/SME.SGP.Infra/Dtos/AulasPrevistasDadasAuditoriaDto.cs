using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AulasPrevistasDadasAuditoriaDto : AuditoriaDto
    {
        public IEnumerable<AulasPrevistasDadasDto> AulasPrevistasPorBimestre { get; set; }
    }
}

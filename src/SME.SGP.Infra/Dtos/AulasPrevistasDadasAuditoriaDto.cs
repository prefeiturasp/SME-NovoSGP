using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AulasPrevistasDadasAuditoriaDto : AuditoriaDto
    {
        public IEnumerable<AulasPrevistasDadasDto> AulasPrevistasPorBimestre { get; set; }
    }
}

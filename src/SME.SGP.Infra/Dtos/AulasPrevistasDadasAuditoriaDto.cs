using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AulasPrevistasDadasAuditoriaDto : AuditoriaDto
    {
        public long Id { get; set; }

        public IEnumerable<AulasPrevistasDadasDto> AulasPrevistasPorBimestre { get; set; }
    }
}

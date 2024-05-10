using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class DashboardAEEPlanosVigentesDto
    {
        public long TotalPlanosVigentes { get; set; }
        public IEnumerable<AEETurmaDto> PlanosVigentes { get; set; }
    }
}

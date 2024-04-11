using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class DashboardAEEPlanosSituacaoDto
    {
        public long TotalPlanosVigentes { get; set; }
        public IEnumerable<AEESituacaoPlanoDto> SituacoesPlanos { get; set; }
    }
}

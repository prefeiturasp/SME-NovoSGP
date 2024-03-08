using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class DashboardAEEEncaminhamentosDto
    {
        public long QtdeEncaminhamentosSituacao { get; set; }

        public long TotalEncaminhamentosAnalise { get; set; }
        public IEnumerable<AEESituacaoEncaminhamentoDto> SituacoesEncaminhamentoAEE { get; set; }
    }
}

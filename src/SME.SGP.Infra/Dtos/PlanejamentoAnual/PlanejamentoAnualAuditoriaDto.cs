using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanejamentoAnualAuditoriaDto
    {
        public PlanejamentoAnualAuditoriaDto()
        {
            PeriodosEscolares = new List<PlanejamentoAnualPeriodoEscolarDto>();
        }
        public List<PlanejamentoAnualPeriodoEscolarDto> PeriodosEscolares { get; set; }
    }
}

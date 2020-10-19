using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanejamentoAnualAuditoriaDto
    {
        public PlanejamentoAnualAuditoriaDto()
        {
            PeriodosEscolares = new List<PlanejamentoAnualPeriodoEscolarDto>();
        }

        public long Id { get; set; }
        public List<PlanejamentoAnualPeriodoEscolarDto> PeriodosEscolares { get; set; }
    }
}

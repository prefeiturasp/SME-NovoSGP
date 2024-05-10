using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanejamentoAnualDto
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public IEnumerable<PlanejamentoAnualPeriodoEscolarDto> PeriodosEscolares { get; set; }
    }
}

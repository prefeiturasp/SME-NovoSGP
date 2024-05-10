using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class MigrarPlanejamentoAnualDto
    {
        public long ComponenteCurricularId { get;  set; }
        public IEnumerable<long> TurmasDestinoIds { get; set; }
        public IEnumerable<long> PlanejamentoPeriodosEscolaresIds { get; set; }
    }
}

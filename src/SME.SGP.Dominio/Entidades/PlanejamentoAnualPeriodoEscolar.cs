using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class PlanejamentoAnualPeriodoEscolar : EntidadeBase
    {
        public long PeriodoEscolarId { get; set; }
        public long PlanejamentoAnualId { get; set; }
        public IEnumerable<PlanejamentoAnualComponente> ComponentesCurriculares { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
    }
}

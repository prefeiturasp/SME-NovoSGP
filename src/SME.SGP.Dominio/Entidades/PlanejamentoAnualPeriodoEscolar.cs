using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class PlanejamentoAnualPeriodoEscolar : EntidadeBase
    {
        protected PlanejamentoAnualPeriodoEscolar()
        {
            ComponentesCurriculares = new List<PlanejamentoAnualComponente>();
        }
        public PlanejamentoAnualPeriodoEscolar(long periodoEscolarId)
        {
            PeriodoEscolarId = periodoEscolarId;
            ComponentesCurriculares = new List<PlanejamentoAnualComponente>();
        }

        public PlanejamentoAnualPeriodoEscolar(long periodoEscolarId, long planejamentoAnualId)
        {
            PeriodoEscolarId = periodoEscolarId;
            PlanejamentoAnualId = planejamentoAnualId;
            ComponentesCurriculares = new List<PlanejamentoAnualComponente>();
        }

        public long PeriodoEscolarId { get; set; }
        public long PlanejamentoAnualId { get; set; }
        public List<PlanejamentoAnualComponente> ComponentesCurriculares { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
        public bool Excluido { get; set; }
    }
}

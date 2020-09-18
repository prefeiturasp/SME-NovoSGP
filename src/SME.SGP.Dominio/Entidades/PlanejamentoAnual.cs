using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class PlanejamentoAnual : EntidadeBase
    {
        public PlanejamentoAnual() { }
        public PlanejamentoAnual(long turmaId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long ComponenteCurricularId { get; set; }
        public bool Migrado { get; set; }
        public long TurmaId { get; set; }
        public IEnumerable<PlanejamentoAnualPeriodoEscolar> PeriodosEscolares { get; set; }
        public Turma Turma { get; set; }
    }
}

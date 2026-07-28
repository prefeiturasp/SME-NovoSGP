using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class PlanejamentoAnual : EntidadeBase
    {
        public PlanejamentoAnual()
        {
            PeriodosEscolares = new List<PlanejamentoAnualPeriodoEscolar>();
        }
        public PlanejamentoAnual(long turmaId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            PeriodosEscolares = new List<PlanejamentoAnualPeriodoEscolar>();
        }

        public long ComponenteCurricularId { get; set; }
        public bool Migrado { get; set; }
        public long TurmaId { get; set; }
        [Computed]
        public List<PlanejamentoAnualPeriodoEscolar> PeriodosEscolares { get; set; }
        [Computed]
        public Turma Turma { get; set; }
    }
}

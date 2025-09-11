using System.Collections;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RelatorioPeriodicoPAPTurma : EntidadeBase
    {
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }
	    public long PeriodoRelatorioId { get; set; }
        public PeriodoRelatorioPAP PeriodoRelatorio { get; set; }
	    public bool Excluido { get; set; }
        public IEnumerable<RelatorioPeriodicoPAPAluno> RelatoriosPeriodicosAlunos { get; set; }
    }
}

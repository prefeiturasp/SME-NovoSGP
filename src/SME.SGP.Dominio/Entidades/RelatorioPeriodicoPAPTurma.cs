using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RelatorioPeriodicoPAPTurma : EntidadeBase
    {
        public long TurmaId { get; set; }
        [Computed]
        public Turma Turma { get; set; }
	    public long PeriodoRelatorioId { get; set; }
        [Computed]
        public PeriodoRelatorioPAP PeriodoRelatorio { get; set; }
	    public bool Excluido { get; set; }
        [Computed]
        public IEnumerable<RelatorioPeriodicoPAPAluno> RelatoriosPeriodicosAlunos { get; set; }
    }
}

using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RelatorioPeriodicoPAPAluno : EntidadeBase
    {
        public string CodigoAluno { get; set; }
	    public string NomeAluno { get; set; }
        public long RelatorioPeriodicoTurmaId { get; set; }
        public RelatorioPeriodicoPAPTurma RelatorioPeriodicoTurma { get; set; }
        public bool Excluido { get; set; }
        public IEnumerable<RelatorioPeriodicoPAPSecao> RelatoriosPeriodicosSecoes { get; set; } 
    }
}

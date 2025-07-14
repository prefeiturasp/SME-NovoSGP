using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
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

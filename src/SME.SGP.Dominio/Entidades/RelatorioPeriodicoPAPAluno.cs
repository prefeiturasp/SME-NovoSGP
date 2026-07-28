using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RelatorioPeriodicoPAPAluno : EntidadeBase
    {
        public string CodigoAluno { get; set; }
	    public string NomeAluno { get; set; }
        public long RelatorioPeriodicoTurmaId { get; set; }
        [Computed]
        public RelatorioPeriodicoPAPTurma RelatorioPeriodicoTurma { get; set; }
        public bool Excluido { get; set; }
        [Computed]
        public IEnumerable<RelatorioPeriodicoPAPSecao> RelatoriosPeriodicosSecoes { get; set; } 
    }
}

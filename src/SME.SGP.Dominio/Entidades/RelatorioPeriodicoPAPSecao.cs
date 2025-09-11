using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RelatorioPeriodicoPAPSecao : EntidadeBase
    {
        public RelatorioPeriodicoPAPSecao()
        {
            Questoes = new List<RelatorioPeriodicoPAPQuestao>();
        }
        public long RelatorioPeriodicoAlunoId { get; set; }
        public RelatorioPeriodicoPAPAluno RelatorioPeriodicoAluno { get; set; }
	    public long SecaoRelatorioPeriodicoId { get; set; }
        public SecaoRelatorioPeriodicoPAP SecaoRelatorioPeriodico { get; set; }
        public bool Concluido { get; set; }
        public bool Excluido { get; set; }
        public List<RelatorioPeriodicoPAPQuestao> Questoes { get; set; }
    }
}

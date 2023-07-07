using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RelatorioPeriodicoPAPSecao : EntidadeBase
    {
        public long RelatorioPeriodicoAlunoId { get; set; }
        public RelatorioPeriodicoPAPAluno RelatorioPeriodicoAluno { get; set; }
	    public long SecaoRelatorioPeriodicoId { get; set; }
        public SecaoRelatorioPeriodicoPAP SecaoRelatorioPeriodico { get; set; }
        public bool Concluido { get; set; }
        public bool Excluido { get; set; }
        public IEnumerable<RelatorioPeriodicoPAPQuestao> RelatoriosPeriodicosQuestoes { get; set; }
    }
}

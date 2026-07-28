using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RelatorioPeriodicoPAPSecao : EntidadeBase
    {
        public RelatorioPeriodicoPAPSecao()
        {
            Questoes = new List<RelatorioPeriodicoPAPQuestao>();
        }
        public long RelatorioPeriodicoAlunoId { get; set; }
        [Computed]
        public RelatorioPeriodicoPAPAluno RelatorioPeriodicoAluno { get; set; }
	    public long SecaoRelatorioPeriodicoId { get; set; }
        [Computed]  
        public SecaoRelatorioPeriodicoPAP SecaoRelatorioPeriodico { get; set; }
        public bool Concluido { get; set; }
        public bool Excluido { get; set; }
        [Computed]
        public List<RelatorioPeriodicoPAPQuestao> Questoes { get; set; }
    }
}

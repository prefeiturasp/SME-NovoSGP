using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RelatorioPeriodicoPAPQuestao : EntidadeBase
    {   
        public long RelatorioPeriodiocoSecaoId { get; set; }
        public RelatorioPeriodicoPAPSecao RelatorioPeriodicoSecao { get; set; }
	    public long QuestaoId { get; set; }
        public Questao Questao { get; set; }
        public bool Excluido { get; set; }
        public IEnumerable<RelatorioPeriodicoPAPResposta> RelaoriosPeriodicosRespostas { get; set; }
    }
}

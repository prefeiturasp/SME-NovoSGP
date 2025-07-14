using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RelatorioPeriodicoPAPQuestao : EntidadeBase
    {   
        public RelatorioPeriodicoPAPQuestao()
        {
            Respostas = new List<RelatorioPeriodicoPAPResposta>();
        }
        public long RelatorioPeriodiocoSecaoId { get; set; }
        public RelatorioPeriodicoPAPSecao RelatorioPeriodicoSecao { get; set; }
	    public long QuestaoId { get; set; }
        public Questao Questao { get; set; }
        public bool Excluido { get; set; }
        public List<RelatorioPeriodicoPAPResposta> Respostas { get; set; }
    }
}

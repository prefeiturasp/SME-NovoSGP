using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RelatorioPeriodicoPAPResposta : EntidadeBase
    {
        public long RelatorioPeriodicoQuestaoId { get; set; }
        public RelatorioPeriodicoPAPQuestao RelatorioPeriodicoQuestao { get; set; }
	    public long? RespostaId { get; set; }
        public OpcaoResposta Resposta { get; set; }
        public long? ArquivoId { get; set; }
        public Arquivo Arquivo { get; set; }
        public string Texto { get; set; }
        public bool Excluido { get; set; }
    }
}

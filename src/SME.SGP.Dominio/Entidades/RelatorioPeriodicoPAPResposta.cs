using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class RelatorioPeriodicoPAPResposta : EntidadeBase
    {
        public long RelatorioPeriodicoQuestaoId { get; set; }
        [Computed]
        public RelatorioPeriodicoPAPQuestao RelatorioPeriodicoQuestao { get; set; }
	    public long? RespostaId { get; set; }
        [Computed]
        public OpcaoResposta Resposta { get; set; }
        public long? ArquivoId { get; set; }
        [Computed]
        public Arquivo Arquivo { get; set; }
        public string Texto { get; set; }
        public bool Excluido { get; set; }
    }
}

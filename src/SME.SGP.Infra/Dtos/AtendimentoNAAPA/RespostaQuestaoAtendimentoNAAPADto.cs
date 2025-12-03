using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class RespostaQuestaoAtendimentoNAAPADto
    {
        public long Id { get; set; }
        public long QuestaoId { get; set; }
        public long? RespostaId { get; set; }
        public string Texto { get; set; }
        public Arquivo Arquivo { get; set; }
    }
}

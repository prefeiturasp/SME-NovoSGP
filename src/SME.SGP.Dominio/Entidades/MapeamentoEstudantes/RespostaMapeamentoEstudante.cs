namespace SME.SGP.Dominio
{
    public class RespostaMapeamentoEstudante : EntidadeBase
    {
        public QuestaoMapeamentoEstudante QuestaoMapeamentoEstudante { get; set; }
        public long QuestaoMapeamentoEstudanteId { get; set; }

        public OpcaoResposta Resposta { get; set; }
        public long? RespostaId { get; set; }

        public Arquivo Arquivo { get; set; }
        public long? ArquivoId { get; set; }

        public string Texto { get; set; }
        public bool Excluido { get; set; }
    }
}

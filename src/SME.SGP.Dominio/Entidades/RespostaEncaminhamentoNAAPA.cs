using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class RespostaEncaminhamentoNAAPA : EntidadeBase
    {
        [Computed]
        public QuestaoEncaminhamentoNAAPA QuestaoEncaminhamento { get; set; }
        public long QuestaoEncaminhamentoId { get; set; }

        [Computed]
        public OpcaoResposta Resposta { get; set; }
        public long? RespostaId { get; set; }

        [Computed]
        public Arquivo Arquivo { get; set; }
        public long? ArquivoId { get; set; }

        public string Texto { get; set; }
        public bool Excluido { get; set; }
    }
}

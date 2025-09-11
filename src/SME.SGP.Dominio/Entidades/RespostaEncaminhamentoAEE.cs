using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RespostaEncaminhamentoAEE : EntidadeBase
    {
        public QuestaoEncaminhamentoAEE QuestaoEncaminhamento { get; set; }
        public long QuestaoEncaminhamentoId { get; set; }

        public OpcaoResposta Resposta { get; set; }
        public long? RespostaId { get; set; }

        public Arquivo Arquivo { get; set; }
        public long? ArquivoId { get; set; }

        public string Texto { get; set; }
        public bool Excluido { get; set; }
    }
}

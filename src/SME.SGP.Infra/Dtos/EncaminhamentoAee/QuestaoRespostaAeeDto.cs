using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class QuestaoRespostaAeeDto
    {
        public long QuestaoId { get; set; }
        public int QuestaoOrder { get; set; }
        public string QuestaoNome { get; set; }
        public string QuestaoObservacao { get; set; }
        public bool QuestaoObrigatorio { get; set; }
        public int QuestaoTipo { get; set; }
        public string QuestaoOpcionais { get; set; }
        public long? OpcaoRespostaId { get; set; }
        public long? QuestaoComplementarId { get; set; }
        public int? OpcaoRespostaOrdem { get; set; }
        public string OpcaoRespostaNome { get; set; }
        public long? RespostaEncaminhamentoId { get; set; }
        public long? RespostaEncaminhamentoOpcaoRespostaId { get; set; }
        public string RespostaTexto { get; set; }
        public long? RespostaArquivoId { get; set; }
        public string ArquivoNome { get; set; }
        public int? ArquivoTipo { get; set; }
        public string ArquivoCodigo { get; set; }
        public string ArquivoTipoConteudo { get; set; }
    }
}

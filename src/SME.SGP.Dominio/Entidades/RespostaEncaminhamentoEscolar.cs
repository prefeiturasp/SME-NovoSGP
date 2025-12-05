using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    [ExcludeFromCodeCoverage]
    public class RespostaEncaminhamentoEscolar : EntidadeBase
    {
        public QuestaoEncaminhamentoEscolar QuestaoEncaminhamento { get; set; }
        public long QuestaoEncaminhamentoId { get; set; }
        public OpcaoResposta Resposta { get; set; }
        public long? RespostaId { get; set; }
        public Arquivo Arquivo { get; set; }
        public long? ArquivoId { get; set; }
        public string Texto { get; set; }
        public bool Excluido { get; set; }
    }
}
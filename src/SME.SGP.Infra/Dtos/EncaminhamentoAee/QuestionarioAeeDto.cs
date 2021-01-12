using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class QuestaoAeeDto
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public bool Obrigatorio { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public string Opcionais { get; set; }
        public OpcaoRespostaAeeDto[] OpcaoResposta { get; set; }
    }
    public class OpcaoRespostaAeeDto
    {

        public QuestaoAeeDto QuestaoComplementar { get; set; }

        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        // resposta
        public long? RespostaEncaminhamentoId { get; set; }
        public Arquivo[] Arquivos { get; set; }
        public string Texto { get; set; }
    }
}

using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{    public class OpcaoRespostaAeeDto
    {

        public QuestaoAeeDto QuestaoComplementar { get; set; }

        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        // resposta
        public long? RespostaId { get; set; }
        public Arquivo[] Arquivos { get; set; }
        public string Texto { get; set; }
    }
}

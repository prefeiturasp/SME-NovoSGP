using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class OpcaoResposta : EntidadeBase
    {
        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }

        public Questao QuestaoComplementar { get; set; }
        public long? QuestaoComplementarId { get; set; }

        public int Ordem { get; set; }
        public string Nome { get; set; }

        public string Observacao { get; set; }
        public bool Excluido { get; set; }
    }
}

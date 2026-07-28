using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class QuestaoEncaminhamentoAEE : EntidadeBase
    {
        public QuestaoEncaminhamentoAEE()
        {
            Respostas = new List<RespostaEncaminhamentoAEE>();
        }

        [Computed]

        public EncaminhamentoAEESecao EncaminhamentoAEESecao { get; set; }
        public long EncaminhamentoAEESecaoId { get; set; }

        [Computed]
        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }

        public bool Excluido { get; set; }

        [Computed]
        public List<RespostaEncaminhamentoAEE> Respostas { get; set; }
    }
}

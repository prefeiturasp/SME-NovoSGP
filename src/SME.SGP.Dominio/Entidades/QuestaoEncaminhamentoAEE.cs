using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class QuestaoEncaminhamentoAEE : EntidadeBase
    {
        public QuestaoEncaminhamentoAEE()
        {
            Respostas = new List<RespostaEncaminhamentoAEE>();
        }

        public EncaminhamentoAEESecao EncaminhamentoAEESecao { get; set; }
        public long EncaminhamentoAEESecaoId { get; set; }

        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }

        public bool Excluido { get; set; }

        public List<RespostaEncaminhamentoAEE> Respostas { get; set; }
    }
}

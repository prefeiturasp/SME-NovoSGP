using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class QuestaoEncaminhamentoNAAPA : EntidadeBase
    {
        public QuestaoEncaminhamentoNAAPA()
        {
            Respostas = new List<RespostaEncaminhamentoNAAPA>();
        }

        [Computed]
        public EncaminhamentoAEESecao EncaminhamentoNAAPASecao { get; set; }
        public long EncaminhamentoNAAPASecaoId { get; set; }

        [Computed]
        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }

        public bool Excluido { get; set; }

        [Computed]
        public List<RespostaEncaminhamentoNAAPA> Respostas { get; set; }
    }
}

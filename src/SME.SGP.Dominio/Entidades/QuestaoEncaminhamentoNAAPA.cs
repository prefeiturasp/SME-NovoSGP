using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class QuestaoEncaminhamentoNAAPA : EntidadeBase
    {
        public QuestaoEncaminhamentoNAAPA()
        {
            Respostas = new List<RespostaEncaminhamentoNAAPA>();
        }

        public EncaminhamentoAEESecao EncaminhamentoNAAPASecao { get; set; }
        public long EncaminhamentoNAAPASecaoId { get; set; }

        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }

        public bool Excluido { get; set; }

        public List<RespostaEncaminhamentoNAAPA> Respostas { get; set; }
    }
}

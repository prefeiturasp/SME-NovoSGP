using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class QuestaoEncaminhamentoAEE : EntidadeBase
    {
        public EncaminhamentoAEESecao EncaminhamentoAEESecao { get; set; }
        public long EncaminhamentoAEESecaoId { get; set; }

        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }

        public bool Excluido { get; set; }
    }
}

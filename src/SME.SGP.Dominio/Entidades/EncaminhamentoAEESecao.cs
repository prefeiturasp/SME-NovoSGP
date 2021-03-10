using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class EncaminhamentoAEESecao : EntidadeBase
    {
        public EncaminhamentoAEESecao()
        {
            Questoes = new List<QuestaoEncaminhamentoAEE>();
        }

        public EncaminhamentoAEE EncaminhamentoAEE { get; set; }
        public long EncaminhamentoAEEId { get; set; }

        public SecaoEncaminhamentoAEE SecaoEncaminhamentoAEE { get; set; }
        public long SecaoEncaminhamentoAEEId { get; set; }

        public bool Concluido { get; set; }
        public bool Excluido { get; set; }

        public List<QuestaoEncaminhamentoAEE> Questoes { get; set; }
    }
}

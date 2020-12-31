using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class SecaoEncaminhamentoAEE : EntidadeBase
    {
        public Questionario Questionario { get; set; }
        public long QuestionarioId { get; set; }

        public bool Excluido { get; set; }
    }
}

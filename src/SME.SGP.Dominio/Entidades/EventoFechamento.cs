using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class EventoFechamento: EntidadeBase
    {
        public long EventoId { get; set; }
        public long FechamentoId { get; set; }
        public bool Excluido { get; set; }
    }
}

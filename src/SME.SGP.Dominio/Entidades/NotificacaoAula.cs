using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class NotificacaoAula
    {
        public long Id { get; set; }
        public long NotificacaoId { get; set; }
        public long AulaId { get; set; }
    }
}

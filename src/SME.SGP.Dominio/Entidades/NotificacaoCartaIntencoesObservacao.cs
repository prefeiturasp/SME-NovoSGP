using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class NotificacaoCartaIntencoesObservacao
    {
        public long Id { get; set; }
        public long NotificacaoId { get; set; }
        public long CartaIntencoesObservacaoId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dto
{
    public class NotificacaoBasicaListaDto
    {
        public IEnumerable<NotificacaoBasicaDto> Notificacoes { get; set; }
        public long QuantidadeTotal { get; set; }
    }
}

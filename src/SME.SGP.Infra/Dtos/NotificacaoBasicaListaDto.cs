using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotificacaoBasicaListaDto
    {
        public IEnumerable<NotificacaoBasicaDto> Notificacoes { get; set; }
        public long QuantidadeNaoLidas { get; set; }
    }
}

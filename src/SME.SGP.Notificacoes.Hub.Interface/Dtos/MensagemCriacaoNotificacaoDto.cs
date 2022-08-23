using System;

namespace SME.SGP.Notificacoes.Hub.Interface
{
    public class MensagemCriacaoNotificacaoDto : MensagemNotificacaoDto
    {
        public string Titulo { get; set; }
        public DateTime Data { get; set; }
        public long Id { get; set; }
    }
}

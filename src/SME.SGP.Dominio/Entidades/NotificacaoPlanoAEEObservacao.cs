using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class NotificacaoPlanoAEEObservacao : EntidadeBase
    {
        public PlanoAEEObservacao PlanoAEEObservacao { get; set; }
        public long PlanoAEEObservacaoId { get; set; }

        public Notificacao Notificacao { get; set; }
        public long NotificacaoId { get; set; }

        public bool Excluido { get; set; }
    }
}

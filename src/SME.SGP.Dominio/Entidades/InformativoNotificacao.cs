﻿namespace SME.SGP.Dominio
{
    public class InformativoNotificacao : EntidadeBase
    {
        public long InformativoId { get; set; }
        public long NotificacaoId { get; set; }
        public bool Excluido { get; set; }
    }
}

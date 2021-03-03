namespace SME.SGP.Dominio
{
    public class NotificacaoPlanoAEE : EntidadeBase
    {
        public NotificacaoPlanoAEE() { }
        public NotificacaoPlanoAEE(long notificacaoId, long planoAEEId, NotificacaoPlanoAEETipo tipo)
        {
            NotificacaoId = notificacaoId;
            PlanoAEEId = planoAEEId;
            Tipo = tipo;
        }

        public NotificacaoPlanoAEETipo Tipo { get; set; }
        public long NotificacaoId { get; set; }
        public long PlanoAEEId { get; set; }
    }
}

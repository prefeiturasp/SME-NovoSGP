namespace SME.SGP.Dominio
{
    public class DevolutivaDiarioBordoNotificacao : EntidadeBase
    {
        public long Id { get; set; }
        public long DevolutivaId { get; set; }
        public long NotificacaoId { get; set; }
    }
}

namespace SME.SGP.Dominio
{
    public class Notificacao : EntidadeBase
    {
        public Notificacao()
        {
            Status = NotificacaoStatus.Pendente;
        }

        public int Ano { get; set; }
        public NotificacaoCategoria Categoria { get; set; }
        public string DreId { get; set; }
        public string EscolaId { get; set; }
        public string Mensagem { get; set; }
        public bool PodeRemover { get; set; }
        public NotificacaoStatus Status { get; set; }
        public NotificacaoTipo Tipo { get; set; }
        public string Titulo { get; set; }
        public string TurmaId { get; set; }
        public string UsuarioId { get; set; }
    }
}
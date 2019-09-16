namespace SME.SGP.Dominio
{
    public class Notificacao : EntidadeBase
    {
        public Notificacao()
        {
            Status = NotificacaoStatus.Pendente;
            Excluida = false;
        }

        public int Ano { get; set; }
        public NotificacaoCategoria Categoria { get; set; }
        public long Codigo { get; set; }
        public string CodigoFormatado { get { return Codigo.ToString().PadLeft(9, '0'); } }
        public bool DeveAprovar { get { return Categoria == NotificacaoCategoria.Workflow_Aprovacao; } }
        public bool DeveMarcarComoLido { get { return Categoria == NotificacaoCategoria.Alerta && Status != NotificacaoStatus.Lida; } }
        public string DreId { get; set; }
        public bool Excluida { get; private set; }
        public string Mensagem { get; set; }
        public bool PodeRemover { get { return Categoria == NotificacaoCategoria.Aviso && !Excluida; } }
        public NotificacaoStatus Status { get; set; }
        public NotificacaoTipo Tipo { get; set; }
        public string Titulo { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }
        public Usuario Usuario { get; set; }
        public long? UsuarioId { get; set; }

        public void MarcarComoLida()
        {
            if (DeveMarcarComoLido && Status != NotificacaoStatus.Lida)
                Status = NotificacaoStatus.Lida;
            else
                throw new NegocioException($"A notificação com id: '{Id}' não pode ser marcada como lida ou já está nesse status.");
        }

        public bool MarcarComoLidaAoObterDetalhe()
        {
            if (Categoria == NotificacaoCategoria.Aviso)
            {
                Status = NotificacaoStatus.Lida;
                return true;
            }
            else return false;
        }

        public void Remover()
        {
            if (PodeRemover)
                Excluida = true;
            else
                throw new NegocioException($"A notificação com id: '{Id}' não pode ser excluída ou já está nesse status.");
        }
    }
}
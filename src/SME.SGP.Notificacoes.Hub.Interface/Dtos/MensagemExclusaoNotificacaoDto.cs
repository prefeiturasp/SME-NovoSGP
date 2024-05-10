namespace SME.SGP.Notificacoes.Hub.Interface
{
    public class MensagemExclusaoNotificacaoDto : MensagemNotificacaoDto
    {
        public MensagemExclusaoNotificacaoDto() { }
        public MensagemExclusaoNotificacaoDto(long codigo, int status, string usuarioRf, bool anoAnterior = false)
        { 
            Codigo = codigo;
            UsuarioRf = usuarioRf;
            Status = status;
            AnoAnterior = anoAnterior;
        }

        public int Status { get; set; }
        public bool AnoAnterior { get; set; }
    }
}

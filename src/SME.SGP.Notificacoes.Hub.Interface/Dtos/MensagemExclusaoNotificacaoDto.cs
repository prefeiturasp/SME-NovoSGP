namespace SME.SGP.Notificacoes.Hub.Interface
{
    public class MensagemExclusaoNotificacaoDto : MensagemNotificacaoDto
    {
        public MensagemExclusaoNotificacaoDto() { }
        public MensagemExclusaoNotificacaoDto(long codigo, int status, string usuarioRf) 
        { 
            Codigo = codigo;
            UsuarioRf = usuarioRf;
            Status = status;
        }

        public int Status { get; set; }
    }
}

namespace SME.SGP.Notificacoes.Hub.Interface
{
    public class MensagemExclusaoNotificacaoDto : MensagemNotificacaoDto
    {
        public MensagemExclusaoNotificacaoDto() { }
        public MensagemExclusaoNotificacaoDto(long codigo, string usuarioRf) 
        { 
            Codigo = codigo;
            UsuarioRf = usuarioRf;
        }

    }
}

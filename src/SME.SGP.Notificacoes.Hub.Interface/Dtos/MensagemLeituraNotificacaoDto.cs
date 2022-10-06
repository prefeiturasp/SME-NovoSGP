namespace SME.SGP.Notificacoes.Hub.Interface
{
    public class MensagemLeituraNotificacaoDto : MensagemNotificacaoDto
    {
        public MensagemLeituraNotificacaoDto() { }
        public MensagemLeituraNotificacaoDto(long codigo, string usuarioRf)
        { 
            Codigo = codigo;
            UsuarioRf = usuarioRf;
        }
    }
}

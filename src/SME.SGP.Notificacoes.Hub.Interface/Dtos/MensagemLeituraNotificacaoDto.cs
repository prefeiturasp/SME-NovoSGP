namespace SME.SGP.Notificacoes.Hub.Interface
{
    public class MensagemLeituraNotificacaoDto : MensagemNotificacaoDto
    {
        public MensagemLeituraNotificacaoDto() { }
        public MensagemLeituraNotificacaoDto(long codigo, string usuarioRf, bool anoAnterior = false)
        { 
            Codigo = codigo;
            UsuarioRf = usuarioRf;
            AnoAnterior = anoAnterior;
        }

        public bool AnoAnterior { get; set; }
    }
}

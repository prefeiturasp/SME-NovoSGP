namespace SME.SGP.Notificacao.Worker
{
    public class MensagemLeituraNotificacaoDto : MensagemNotificacaoDto
    {
        public MensagemLeituraNotificacaoDto(long codigo, string usuarioRf) : base(codigo, usuarioRf)
        {
        }
    }
}

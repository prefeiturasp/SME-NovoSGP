namespace SME.SGP.Notificacao.Worker
{
    public class MensagemExclusaoNotificacaoDto : MensagemNotificacaoDto
    {
        public MensagemExclusaoNotificacaoDto(long codigo, string usuarioRf) :base(codigo, usuarioRf)
        {
        }
    }
}

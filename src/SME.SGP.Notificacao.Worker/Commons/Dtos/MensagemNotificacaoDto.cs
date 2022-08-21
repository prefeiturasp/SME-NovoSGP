namespace SME.SGP.Notificacao.Worker
{
    public abstract class MensagemNotificacaoDto
    {
        public MensagemNotificacaoDto(long codigo, string usuarioRf)
        {
            Codigo = codigo;
            UsuarioRf = usuarioRf;
        }

        public long Codigo { get; }
        public string UsuarioRf { get; }
    }
}

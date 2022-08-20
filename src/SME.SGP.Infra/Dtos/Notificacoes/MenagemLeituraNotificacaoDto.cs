namespace SME.SGP.Infra
{
    public class MenagemLeituraNotificacaoDto
    {
        public MenagemLeituraNotificacaoDto(long codigo, string usuarioRf)
        {
            Codigo = codigo;
            UsuarioRf = usuarioRf;
        }

        public long Codigo { get; }
        public string UsuarioRf { get; }
    }
}

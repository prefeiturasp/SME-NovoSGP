using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class NotificacaoUsuarioDto
    {
        public long Id { get; set; }
        public long Codigo { get; set; }
        public NotificacaoStatus Status { get; set; }
        public string UsuarioRf { get; set; }
    }
}

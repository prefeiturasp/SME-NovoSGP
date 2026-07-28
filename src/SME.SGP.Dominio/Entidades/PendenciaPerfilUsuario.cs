using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PendenciaPerfilUsuario : EntidadeBase
    {
        public PendenciaPerfilUsuario() { }
        public PendenciaPerfilUsuario(long pendenciaPerfilId, long usuarioId, PerfilUsuario perfilCodigo)
        {
            PendenciaPerfilId = pendenciaPerfilId;
            UsuarioId = usuarioId;
            PerfilCodigo = perfilCodigo;
        }

        [Computed]
        public PendenciaPerfil PendenciaPerfil { get; set; }
        public long PendenciaPerfilId { get; set; }
        public PerfilUsuario PerfilCodigo { get; set; }
        public long UsuarioId { get; set; }
        [Computed]
        public Usuario Usuario { get; set; }
    }
}

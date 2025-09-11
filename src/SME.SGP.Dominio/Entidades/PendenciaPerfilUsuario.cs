using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PendenciaPerfilUsuario : EntidadeBase
    {
        public PendenciaPerfilUsuario() { }
        public PendenciaPerfilUsuario(long pendenciaPerfilId, long usuarioId, PerfilUsuario perfilCodigo)
        {
            PendenciaPerfilId = pendenciaPerfilId;
            UsuarioId = usuarioId;
            PerfilCodigo = perfilCodigo;
        }

        public PendenciaPerfil PendenciaPerfil { get; set; }
        public long PendenciaPerfilId { get; set; }
        public PerfilUsuario PerfilCodigo { get; set; }
        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}

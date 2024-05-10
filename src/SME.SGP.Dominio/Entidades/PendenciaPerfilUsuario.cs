using System;
using System.Collections.Generic;
using System.Text;

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

        public PendenciaPerfil PendenciaPerfil { get; set; }
        public long PendenciaPerfilId { get; set; }
        public PerfilUsuario PerfilCodigo { get; set; }
        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class PendenciaPerfilUsuario : EntidadeBase
    {
        public PendenciaPerfil PendenciaPerfil { get; set; }
        public long PendenciaPerfilId { get; set; }
        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class PendenciaUsuario : EntidadeBase
    {
        public long PendenciaId { get; set; }
        public Pendencia Pendencia { get; set; }
        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}

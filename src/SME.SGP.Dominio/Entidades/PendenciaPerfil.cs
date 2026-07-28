using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class PendenciaPerfil : EntidadeBase
    {
        public PendenciaPerfil()
        {
            PendenciasPerfilUsuarios = new List<PendenciaPerfilUsuario>();
        }

        public PerfilUsuario PerfilCodigo { get; set; }
        public long PendenciaId { get; set; }
        [Computed]
        public Pendencia Pendencia { get; set; }
        [Computed]

        public List<PendenciaPerfilUsuario> PendenciasPerfilUsuarios { get; set; }

        public void AdicionaPendenciaPerfilUsuario(PendenciaPerfilUsuario pendenciaPerfilUsuario)
        {
            if (pendenciaPerfilUsuario.NaoEhNulo())
                PendenciasPerfilUsuarios.Add(pendenciaPerfilUsuario);
        }
    }
}

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
        public Pendencia Pendencia { get; set; }

        public List<PendenciaPerfilUsuario> PendenciasPerfilUsuarios { get; set; }

        public void AdicionaPendenciaPerfilUsuario(PendenciaPerfilUsuario pendenciaPerfilUsuario)
        {
            if (pendenciaPerfilUsuario.NaoEhNulo())
                PendenciasPerfilUsuarios.Add(pendenciaPerfilUsuario);
        }
    }
}

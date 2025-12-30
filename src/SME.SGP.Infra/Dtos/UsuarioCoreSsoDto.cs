using System;

namespace SME.SGP.Infra
{
    public class UsuarioCoreSsoDto
    {
        public string Login { get; set; }
        public string Nome { get; set; }
        public Guid Perfil { get; set; }
    }
}

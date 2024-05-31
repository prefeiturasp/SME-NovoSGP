using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class UsuarioPerfilsAbrangenciaDto
    {
        public string UsuarioRf { get; set; }

        public List<PerfilsAbrangenciaDto> Perfils { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.Abrangencia
{
    public class AbrangenciaAgrupadaUsuarioPerfisDto
    {
        public string Login { get; set; }
        public List<Guid> Perfil { get; set; }
    }
}

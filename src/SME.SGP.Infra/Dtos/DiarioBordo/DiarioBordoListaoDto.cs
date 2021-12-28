using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class DiarioBordoListaoDto
    {
        public string Planejamento { get; set; }
        public string ReflexoesReplanejamento { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}

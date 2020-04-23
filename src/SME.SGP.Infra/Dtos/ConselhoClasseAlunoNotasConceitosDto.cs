using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ConselhoClasseAlunoNotasConceitosDto
    {
        public ConselhoClasseAlunoNotasConceitosDto()
        {
            ComponentesCurriculares = new List<ConselhoClasseComponenteFrequenciaDto>();
        }

        public ConselhoClasseComponenteRegenciaFrequenciaDto ComponenteRegencia { get; set; }
        public List<ConselhoClasseComponenteFrequenciaDto> ComponentesCurriculares { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ConselhoClasseComponenteRegenciaFrequenciaDto
    {
        public ConselhoClasseComponenteRegenciaFrequenciaDto()
        {
            ComponentesCurriculares = new List<ConselhoClasseNotasComponenteRegenciaDto>();
        }

        public int QuantidadeAulas { get; set; }
        public int Faltas { get; set; }
        public int AusenciasCompensadas { get; set; }
        public double Frequencia { get; set; }
        public List<ConselhoClasseNotasComponenteRegenciaDto> ComponentesCurriculares { get; set; }
    }
}

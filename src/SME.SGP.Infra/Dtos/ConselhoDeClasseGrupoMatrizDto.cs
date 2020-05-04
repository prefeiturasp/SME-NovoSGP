using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ConselhoDeClasseGrupoMatrizDto
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public List<ConselhoDeClasseComponenteSinteseDto> ComponenteSinteses { get; set; }
    }
}

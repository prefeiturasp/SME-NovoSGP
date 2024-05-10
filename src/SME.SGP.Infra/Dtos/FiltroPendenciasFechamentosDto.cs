using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPendenciasFechamentosDto
    {
        public string TurmaCodigo { get; set; }
        public int Bimestre { get; set; }
        public long ComponenteCurricularId { get; set; }
    }
}

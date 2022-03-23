using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PeriodoSeparadoRegenciaDto
    {
        public int Semana { get; set; }
        public DateTime PrimeiroDia { get; set; }
        public DateTime UltimoDia { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class ConselhoClasseParecerAno : EntidadeBase
    {
        public long ParecerId { get; set; }
        public int AnoTurma { get; set; }
        public int Modalidade { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime? FimVigencia { get; set; }
    }
}

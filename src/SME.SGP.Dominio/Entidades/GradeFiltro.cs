using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class GradeFiltro : EntidadeBase
    {
        public long GradeId { get; set; }
        public Grade Grade { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public Modalidade Modalidade { get; set; }
        public int DuracaoTurno { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class GradeDisciplina : EntidadeBase
    {
        public long GradeId { get; set; }
        public Grade Grade { get; set; }
        public int Ano { get; set; }
        public long ComponenteCurricularId { get; set; }
        public int QuantidadeAulas { get; set; }
    }
}

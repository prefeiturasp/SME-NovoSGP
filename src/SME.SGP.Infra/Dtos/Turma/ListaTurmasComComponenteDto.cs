using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ListaTurmasComComponenteDto
    {
        public int Id { get; set; }
        public string NomeTurma { get; set; }
        public long TurmaCodigo { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public string Turno { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AulasPorTurmaDisciplinaDto
    {
        public int ProfessorId { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataAula { get; set; }
    }
}

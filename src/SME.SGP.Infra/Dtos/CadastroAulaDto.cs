using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CadastroAulaDto
    {
        public bool PodeCadastrarAula { get; set; }
        public GradeComponenteTurmaAulasDto Grade { get; set; }
    }
}

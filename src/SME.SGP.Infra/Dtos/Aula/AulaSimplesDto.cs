using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AulaSimplesDto
    {
        public long AulaId { get; set; }
        public bool AulaCJ { get; set; }
        public string ProfessorRf { get; set; }
        public string CriadoPor { get; set; }
        public bool PossuiFrequenciaRegistrada { get; set; }
    }
}

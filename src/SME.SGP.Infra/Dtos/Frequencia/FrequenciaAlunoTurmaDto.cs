using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class FrequenciaAlunoTurmaDto
    {
        public long RegistroFrequenciaAlunoId { get; set; }
        public DateTime DataAula { get; set; }
        public long AulaId { get; set; }
        public string DisciplinaCodigo { get; set; }
        public int Valor { get; set; }
    }
}

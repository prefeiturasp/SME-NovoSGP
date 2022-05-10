using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class AulaComComponenteDto
    {
        public long Id { get; set; }
        public string TurmaId { get; set; }
        public long ComponenteId { get; set; }
        public int PeriodoEscolarId { get; set; }
    }
}

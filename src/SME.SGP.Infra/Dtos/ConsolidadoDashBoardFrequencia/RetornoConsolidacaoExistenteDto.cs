using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class RetornoConsolidacaoExistenteDto
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public int Tipo { get; set; }
        public int Presentes { get; set; }
        public int Remotos { get; set; }
        public int Ausentes { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class TurmaComComponentesInfantilDto
    {
        public string TurmaId { get; set; }
        public long[] ComponentesId { get; set; }

        public TurmaComComponentesInfantilDto()
        {
            ComponentesId = new long[] { };
        }
    }
}

using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class ConsolidacaoPorTurmaDashBoardFrequencia
    {
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public long TurmaId { get; set; }
        public DateTime DataAula { get; set; }
    }
}

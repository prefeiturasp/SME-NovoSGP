using System;

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

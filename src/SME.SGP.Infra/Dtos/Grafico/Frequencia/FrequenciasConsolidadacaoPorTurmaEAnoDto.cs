using System;

namespace SME.SGP.Infra
{
    public class FrequenciasConsolidadacaoPorTurmaEAnoDto
    {
        public int AnoLetivo { get; set; } 
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public string AnoTurma { get; set; }
        public int Semestre { get; set; }
        public DateTime DataAula { get; set; }
        public bool VisaoDre { get; set; }
    }
}

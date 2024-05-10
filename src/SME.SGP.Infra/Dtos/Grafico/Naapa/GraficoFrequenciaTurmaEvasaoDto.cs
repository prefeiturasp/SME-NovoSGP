using System;

namespace SME.SGP.Infra
{
    public class GraficoFrequenciaTurmaEvasaoDto : GraficoBaseDto
    {
        public DateTime? DataUltimaConsolidacao { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string TurmaCodigo { get; set; }
    }
}

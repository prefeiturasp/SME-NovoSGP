using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class GraficoAtendimentoNAAPADto
    {
        public GraficoAtendimentoNAAPADto()
        {
            Graficos = new List<GraficoBaseDto>();
        }
        public DateTime? DataUltimaConsolidacao { get; set; }
        public long TotaEncaminhamento { get; set; }
        public List<GraficoBaseDto> Graficos { get; set; }
    }
}
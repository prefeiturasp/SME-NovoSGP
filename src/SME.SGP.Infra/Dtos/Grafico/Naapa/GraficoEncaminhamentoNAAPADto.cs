using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class GraficoEncaminhamentoNAAPADto
    {

        public DateTime? DataUltimaConsolidacao { get; set; }
        public List<GraficoBaseDto> Graficos { get; set; }
    }
}
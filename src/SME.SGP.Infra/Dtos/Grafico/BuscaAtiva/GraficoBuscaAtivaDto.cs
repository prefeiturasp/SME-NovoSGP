using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class GraficoBuscaAtivaDto
    {
        public GraficoBuscaAtivaDto()
        {
            Graficos = new List<GraficoBaseDto>();
        }
        public DateTime? DataUltimaConsolidacao { get; set; }
        public List<GraficoBaseDto> Graficos { get; set; }
    }
}
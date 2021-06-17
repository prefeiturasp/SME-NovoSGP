using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class FiltroDashboardItineranciaDto
    {
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public string RF { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
    }
}

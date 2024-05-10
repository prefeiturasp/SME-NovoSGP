using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class DashboardItineranciaVisitaPaais
    {
        public long TotalRegistro { get; set; }
        public IEnumerable<DashboardItineranciaDto> DashboardItinerancias {  get; set; }
    }
}

using System;

namespace SME.SGP.Infra.Dtos
{
    public class FiltroDataMetricasDto
    {
        public FiltroDataMetricasDto(DateTime data, bool ignorarRecheckCargaMetricas = false) 
        {
            Data = data;
            IgnorarRecheckCargaMetricas = ignorarRecheckCargaMetricas;
        }
        public DateTime Data { get; set; }
        public bool IgnorarRecheckCargaMetricas { get; set; }

    }
}

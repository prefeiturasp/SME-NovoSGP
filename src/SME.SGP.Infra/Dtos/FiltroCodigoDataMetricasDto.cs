using System;

namespace SME.SGP.Infra
{
    public class FiltroCodigoDataMetricasDto
    {
        public FiltroCodigoDataMetricasDto(string codigo, DateTime data, bool ignorarRecheckCargaMetricas = false)
        {
            Codigo = codigo;
            Data = data;
            IgnorarRecheckCargaMetricas = ignorarRecheckCargaMetricas;
        }
        public string Codigo { get; set; }
        public DateTime Data { get; set; }
        public bool IgnorarRecheckCargaMetricas { get; set; }
    }
}

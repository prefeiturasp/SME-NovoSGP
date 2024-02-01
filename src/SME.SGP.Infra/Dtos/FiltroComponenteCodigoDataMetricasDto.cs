using System;

namespace SME.SGP.Infra
{
    public class FiltroComponenteCodigoDataMetricasDto
    {
        public FiltroComponenteCodigoDataMetricasDto(string codigo, DateTime data, string codigoTurma)
        {
            Codigo = codigo;
            Data = data;
            CodigoTurma = codigoTurma;
        }
        public string Codigo { get; set; }
        public DateTime Data { get; set; }
        public string CodigoTurma { get; set; }
    }
}

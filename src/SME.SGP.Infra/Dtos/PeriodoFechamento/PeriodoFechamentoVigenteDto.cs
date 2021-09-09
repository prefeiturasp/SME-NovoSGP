using System;

namespace SME.SGP.Infra
{
    public class PeriodoFechamentoVigenteDto
    {
        public string Calendario { get; set; }
        public int AnoLetivo { get; set; }
        public int Bimestre { get; set; }
        public DateTime PeriodoFechamentoInicio { get; set; }
        public DateTime PeriodoFechamentoFim { get; set; }
        public DateTime PeriodoEscolarInicio { get; set; }
        public DateTime PeriodoEscolarFim { get; set; }
    }
}

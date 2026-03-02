using System;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Frequencia
{
    public class ConsolidacaoFrequenciaSemanalDto
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
        public int TotalEstudantes { get; set; }
        public int TotalPresentes { get; set; }
        public decimal PercentualFrequencia { get; set; }
        public DateTime DataAula { get; set; }
    }
}

using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoGlobal
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Modalidade { get; set; }
        public int TotalAulas { get; set; }
        public int TotalAusencias { get; set; }
        public decimal PercentualFrequencia { get; set; }
        public int TotalAlunos { get; set; }
        public int AnoLetivo { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    }
}

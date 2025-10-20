using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoFrequenciaDiariaDreTeste
    {
        public long Id { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public NivelFrequenciaEnum NivelFrequencia { get; set; }
        public string Ue { get; set; }
        public int AnoLetivo { get; set; }
        public int TotalEstudantes { get; set; }
        public int TotalPresentes { get; set; }
        public decimal PercentualFrequencia { get; set; }
        public DateTime DataAula { get; set; }
        public long TotalAulas { get; set; }
        public long TotalPresencas { get; set; }
        public long TotalAusencias { get; set; }
        public decimal Percentual { get; set; }
        public string NivelFrequencia { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}

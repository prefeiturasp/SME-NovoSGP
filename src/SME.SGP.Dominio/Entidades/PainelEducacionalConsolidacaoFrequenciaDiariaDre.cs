using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoFrequenciaDiariaDreTeste
    {
        public long Id { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Ue { get; set; }
        public DateTime DataAula { get; set; }
        public long TotalAulas { get; set; }
        public long TotalPresencas { get; set; }
        public long TotalAusencias { get; set; }
        public decimal Percentual { get; set; }
        public string NivelFrequencia { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}

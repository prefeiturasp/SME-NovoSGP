using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoAprovacao
    {
        public string CodigoDre { get; set; }
        public string SerieAno { get; set; }
        public string Modalidade { get; set; }
        public int TotalPromocoes { get; set; }
        public int TotalRetencoesAusencias { get; set; }
        public int TotalRetencoesNotas { get; set; }
        public int AnoLetivo { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTimeExtension.HorarioBrasilia();
    }
}

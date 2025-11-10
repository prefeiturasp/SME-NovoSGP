using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoAprovacaoUe
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Turma { get; set; }
        public string Modalidade { get; set; }
        public int TotalPromocoes { get; set; }
        public int TotalRetencoesAusencias { get; set; }
        public int TotalRetencoesNotas { get; set; }
        public int AnoLetivo { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTimeExtension.HorarioBrasilia();
    }
}

using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoAbandonoUe
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string CodigoTurma { get; set; }
        public string NomeTurma { get; set; }
        public string Modalidade { get; set; }
        public int QuantidadeDesistencias { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    }
}
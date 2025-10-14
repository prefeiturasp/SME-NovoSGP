using System;

namespace SME.SGP.Dominio
{
    public class PainelEducacionalAbandonoUe
    {
        public long Id { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string CodigoTurma { get; set; }
        public string NomeTurma { get; set; }
        public string Modalidade { get; set; }
        public int QuantidadeDesistencias { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}

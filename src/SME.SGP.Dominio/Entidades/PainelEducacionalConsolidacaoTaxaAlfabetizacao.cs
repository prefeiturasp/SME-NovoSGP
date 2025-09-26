using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoTaxaAlfabetizacao
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
        public decimal TaxaAlfabetizacao { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    }
}

using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class TaxaAlfabetizacao : EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public string CodigoEOLEscola { get; set; }
        public decimal Taxa { get; set; }
    }
}

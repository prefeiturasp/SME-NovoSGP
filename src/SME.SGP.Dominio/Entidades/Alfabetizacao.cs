using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class Alfabetizacao : EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public string CodigoEOLEscola { get; set; }
        public decimal TaxaAlfabetizacao { get; set; }
    }
}

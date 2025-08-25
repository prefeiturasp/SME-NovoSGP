using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ArquivoIdep : EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public int SerieAno { get; set; }
        public string CodigoEOLEscola { get; set; }
        public decimal Nota { get; set; }
    }
}

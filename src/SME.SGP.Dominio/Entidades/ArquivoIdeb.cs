using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ArquivoIdeb : EntidadeBase
    {
        public int SerieAno { get; set; }
        public string CodigoEOLEscola { get; set; }
        public double Nota { get; set; }
    }
}

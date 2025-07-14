using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ComponenteCurricularJurema : EntidadeBase
    {
        public long CodigoEOL { get; set; }
        public long CodigoJurema { get; set; }
        public string DescricaoEOL { get; set; }
    }
}
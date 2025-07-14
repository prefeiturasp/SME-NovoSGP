using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class TipoReuniaoNAAPA : EntidadeBase
    {
        public string Titulo { get; set; }
        public bool Excluido { get; set; }
    }
}

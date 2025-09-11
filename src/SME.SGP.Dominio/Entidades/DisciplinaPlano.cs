using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class DisciplinaPlano : EntidadeBase
    {
        public long DisciplinaId { get; set; }
        public long PlanoId { get; set; }
    }
}
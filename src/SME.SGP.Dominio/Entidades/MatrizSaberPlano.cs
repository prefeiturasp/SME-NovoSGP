using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class MatrizSaberPlano : EntidadeBase
    {
        public long MatrizSaberId { get; set; }
        public long PlanoId { get; set; }
    }
}
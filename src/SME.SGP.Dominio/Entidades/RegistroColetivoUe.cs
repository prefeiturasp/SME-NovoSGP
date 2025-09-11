using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RegistroColetivoUe : EntidadeBase
    {
        public long UeId { get; set; }
        public long RegistroColetivoId { get; set; }
    }
}

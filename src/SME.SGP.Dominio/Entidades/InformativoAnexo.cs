using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class InformativoAnexo
    {
        public long Id { get; set; }
        public long InformativoId { get; set; }
        public long ArquivoId { get; set; }
    }
}
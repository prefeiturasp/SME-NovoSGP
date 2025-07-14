using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RegistroColetivoAnexo : EntidadeBase
    {
        public long RegistroColetivoId { get; set; }
        public Arquivo Arquivo { get; set; }
        public long ArquivoId { get; set; }
        public bool Excluido { get; set; }
    }
}

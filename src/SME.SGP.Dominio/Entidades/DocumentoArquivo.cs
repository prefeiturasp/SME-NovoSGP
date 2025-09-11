using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class DocumentoArquivo
    {
        public long Id { get; set; }
        public long DocumentoId { get; set; }
        public long ArquivoId { get; set; }
    }
}
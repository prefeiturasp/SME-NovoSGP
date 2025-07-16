using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class AlunoFoto : EntidadeBase
    {
        public Arquivo Arquivo { get; set; }
        public long ArquivoId { get; set; }
        public Arquivo Miniatura { get; set; }
        public long? MiniaturaId { get; set; }
        public string AlunoCodigo { get; set; }
        public bool Excluido { get; set; }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class Arquivo : EntidadeBase
    {
        public string Nome { get; set; }
        public Guid Codigo { get; set; }
        public string TipoConteudo { get; set; }
        public TipoArquivo Tipo { get; set; }
    }
}

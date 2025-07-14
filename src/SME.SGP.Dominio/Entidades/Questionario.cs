using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class Questionario : EntidadeBase
    {
        public string Nome { get; set; }
        public TipoQuestionario Tipo { get; set; }
        public bool Excluido { get; set; }
    }
}

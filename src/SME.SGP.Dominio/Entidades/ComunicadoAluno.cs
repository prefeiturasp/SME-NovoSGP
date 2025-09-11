using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ComunicadoAluno : EntidadeBase
    {
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public long ComunicadoId { get; set; }
    }
}

using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PlanoAEETurmaAluno : EntidadeBase
    {
        public long PlanoAEEId { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
    }
}

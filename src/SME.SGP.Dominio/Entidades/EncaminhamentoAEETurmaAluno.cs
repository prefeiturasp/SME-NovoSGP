using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class EncaminhamentoAEETurmaAluno : EntidadeBase
    {
        public long EncaminhamentoAEEId { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
    }
}

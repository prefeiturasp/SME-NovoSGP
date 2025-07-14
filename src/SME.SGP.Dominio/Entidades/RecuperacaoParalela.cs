using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RecuperacaoParalela : EntidadeBase
    {
        public long Aluno_id { get; set; }
        public bool Excluido { get; set; }
        public long TurmaId { get; set; }
        public long TurmaRecuperacaoParalelaId { get; set; }
        public int AnoLetivo { get; set; }
    }
}
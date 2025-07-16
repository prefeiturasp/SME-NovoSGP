using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class GradeFiltro : EntidadeBase
    {
        public long GradeId { get; set; }
        public Grade Grade { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public Modalidade Modalidade { get; set; }
        public int DuracaoTurno { get; set; }
    }
}

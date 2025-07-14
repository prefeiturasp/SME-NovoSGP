using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class AulaPrevista : EntidadeBase
    {
        public long TipoCalendarioId { get; set; }
        public string DisciplinaId { get; set; }
        public string TurmaId { get; set; }
    }
}


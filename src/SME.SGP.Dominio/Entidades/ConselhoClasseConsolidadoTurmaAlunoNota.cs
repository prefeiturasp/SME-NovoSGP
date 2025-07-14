using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConselhoClasseConsolidadoTurmaAlunoNota 
    {
        public long Id { get; set; }
        public long ConselhoClasseConsolidadoTurmaAlunoId { get; set; }
        public int? Bimestre { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public long? ComponenteCurricularId { get; set; }
    }
}

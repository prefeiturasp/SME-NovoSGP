using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConsolidacaoDiariosBordo
    {
        public long Id { get; set; }
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public int QuantidadePreenchidos { get; set; }
        public int QuantidadePendentes { get; set; }
    }
}

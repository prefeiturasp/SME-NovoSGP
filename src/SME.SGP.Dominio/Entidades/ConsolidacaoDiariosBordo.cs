using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class ConsolidacaoDiariosBordo
    {
        [Key]
        public long Id { get; set; }
        [Computed]
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public int QuantidadePreenchidos { get; set; }
        public int QuantidadePendentes { get; set; }
    }
}

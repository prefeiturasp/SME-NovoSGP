using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class HistoricoNotaConselhoClasse
    {
        public long Id {get; set;}
        public long HistoricoNotaId { get; set; }
        public long ConselhoClasseNotaId { get; set; }
    }
}

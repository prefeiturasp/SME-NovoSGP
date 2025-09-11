using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class EventoFechamento: EntidadeBase
    {
        public Evento Evento { get; set; }
        public long EventoId { get; set; }
        public long FechamentoId { get; set; }
        public bool Excluido { get; set; }
    }
}

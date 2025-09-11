using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ItineranciaEvento : EntidadeBase
    {
        public ItineranciaEvento() { }
        public ItineranciaEvento(long itineranciaId, long eventoId)
        {
            ItineranciaId = itineranciaId;
            EventoId = eventoId;
        }

        public long ItineranciaId { get; set; }
        public long EventoId { get; set; }

        public bool Excluido { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
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

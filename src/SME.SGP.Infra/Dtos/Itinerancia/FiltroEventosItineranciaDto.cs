using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroEventosItineranciaDto
    {
        public FiltroEventosItineranciaDto(long tipoCalendarioId, long itineranciaId)
        {
            TipoCalendarioId = tipoCalendarioId;
            ItineranciaId = itineranciaId;
        }

        public long TipoCalendarioId { get; set; }
        public long ItineranciaId { get; set; }
    }
}

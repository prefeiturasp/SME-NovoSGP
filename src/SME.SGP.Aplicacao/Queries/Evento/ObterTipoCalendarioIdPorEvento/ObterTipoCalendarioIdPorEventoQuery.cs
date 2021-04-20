using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorEventoQuery : IRequest<long>
    {
        public ObterTipoCalendarioIdPorEventoQuery(long eventoId)
        {
            EventoId = eventoId;
        }

        public long EventoId { get; }
    }
}

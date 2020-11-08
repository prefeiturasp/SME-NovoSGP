using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosPorTipoCalendarioIdTurmaIdEDataQuery : IRequest<IEnumerable<Evento>>
    {
        public ObterEventosPorTipoCalendarioIdTurmaIdEDataQuery(long tipoCalendarioId, DateTime periodoInicio, DateTime periodoFim, long? turmaId)
        {
            TipoCalendarioId = tipoCalendarioId;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
            TurmaId = turmaId;
        }

        public long TipoCalendarioId { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public long? TurmaId { get; set; }
    }
}

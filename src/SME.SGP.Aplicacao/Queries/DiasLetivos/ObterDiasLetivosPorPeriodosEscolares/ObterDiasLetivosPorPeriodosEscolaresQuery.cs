using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasLetivosPorPeriodosEscolaresQuery : IRequest<List<DiaLetivoDto>>
    {
        public ObterDiasLetivosPorPeriodosEscolaresQuery(IEnumerable<Dominio.PeriodoEscolar> periodosEscolares, long tipoCalendarioId)
        {
            PeriodosEscolares = periodosEscolares;
            TipoCalendarioId = tipoCalendarioId;
        }

        public ObterDiasLetivosPorPeriodosEscolaresQuery(Dominio.PeriodoEscolar periodoEscolar, long tipoCalendarioId)
        {
            PeriodosEscolares = new List<Dominio.PeriodoEscolar> { periodoEscolar };
            TipoCalendarioId = tipoCalendarioId;
        }

        public long TipoCalendarioId { get; set; }
        public IEnumerable<Dominio.PeriodoEscolar> PeriodosEscolares { get; set; }
    }
}

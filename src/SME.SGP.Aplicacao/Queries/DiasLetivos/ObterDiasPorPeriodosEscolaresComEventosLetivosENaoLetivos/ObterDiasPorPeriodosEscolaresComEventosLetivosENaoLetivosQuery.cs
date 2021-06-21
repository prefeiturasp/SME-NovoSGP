using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery : IRequest<List<DiaLetivoDto>>
    {
        public ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(IEnumerable<Dominio.PeriodoEscolar> periodosEscolares, long tipoCalendarioId, bool desconsiderarCriacaoDiaLetivoProximasUes = false)
        {
            PeriodosEscolares = periodosEscolares;
            TipoCalendarioId = tipoCalendarioId;
            DesconsiderarCriacaoDiaLetivoProximasUes = desconsiderarCriacaoDiaLetivoProximasUes;
        }

        public long TipoCalendarioId { get; set; }
        public IEnumerable<Dominio.PeriodoEscolar> PeriodosEscolares { get; set; }
        public bool DesconsiderarCriacaoDiaLetivoProximasUes { get; set; }
    }
}

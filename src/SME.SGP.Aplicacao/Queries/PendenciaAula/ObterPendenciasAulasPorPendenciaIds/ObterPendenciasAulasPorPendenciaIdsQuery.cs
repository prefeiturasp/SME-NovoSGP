using MediatR;
using SME.SGP.Infra.Dtos.PendenciaAula;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PendenciaAula.ObterPendenciasAulasPorPendenciaIds
{
    public class ObterPendenciasAulasPorPendenciaIdsQuery : IRequest<IEnumerable<PendenciaAulasDto>>
    {
        public ObterPendenciasAulasPorPendenciaIdsQuery(long[] pendenciaIds)
        {
            PendenciaIds = pendenciaIds ?? throw new ArgumentNullException(nameof(pendenciaIds));
        }
        public long[] PendenciaIds { get; set; }
    }
}

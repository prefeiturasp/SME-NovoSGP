using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulasPorPendenciaQuery : IRequest<IEnumerable<PendenciaAulaDto>>
    {
        public ObterPendenciasAulasPorPendenciaQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }
}

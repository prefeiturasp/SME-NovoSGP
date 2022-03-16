using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorIdQuery : IRequest<IEnumerable<PendenciaAulaDto>>
    {
        public ObterPendenciasAulaPorIdQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }
}

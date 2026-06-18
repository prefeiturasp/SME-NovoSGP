using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMuralAvisoPorAulaIdQuery : IRequest<IEnumerable<MuralAvisosRetornoDto>>
    {
        public long AulaId { get; set; }

        public ObterMuralAvisoPorAulaIdQuery(long aulaId)
        {
            AulaId = aulaId;
        }
    }
}
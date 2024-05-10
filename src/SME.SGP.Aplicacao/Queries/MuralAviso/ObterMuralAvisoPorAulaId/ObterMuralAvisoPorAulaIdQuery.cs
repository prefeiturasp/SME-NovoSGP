using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

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
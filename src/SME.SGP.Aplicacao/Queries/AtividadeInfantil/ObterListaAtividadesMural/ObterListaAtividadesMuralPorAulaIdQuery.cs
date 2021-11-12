using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterListaAtividadesMuralPorAulaIdQuery : IRequest<IEnumerable<AtividadeInfantilDto>>
    {
        public long AulaId { get; set; }

        public ObterListaAtividadesMuralPorAulaIdQuery(long aulaId)
        {
            AulaId=aulaId;
        }
    }
}

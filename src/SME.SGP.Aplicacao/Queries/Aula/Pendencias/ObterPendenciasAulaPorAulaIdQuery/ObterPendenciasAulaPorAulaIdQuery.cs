using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdQuery : IRequest<long[]>
    {
        public ObterPendenciasAulaPorAulaIdQuery(long aulaId)
        {
            AulaId = aulaId;
        }
        public long AulaId { get; set; }


    }
}

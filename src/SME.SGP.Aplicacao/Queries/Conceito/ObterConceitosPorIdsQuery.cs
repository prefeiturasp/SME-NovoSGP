using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConceitosPorIdsQuery : IRequest<IEnumerable<Conceito>>
    {
        public long[] Ids { get; set; }

        public ObterConceitosPorIdsQuery(long[] ids)
        {
            Ids = ids;
        }
    }
}

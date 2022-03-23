using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTodasUesIdsQuery : IRequest<IEnumerable<long>>
    {
        public ObterTodasUesIdsQuery()
        {
        }
    }
}

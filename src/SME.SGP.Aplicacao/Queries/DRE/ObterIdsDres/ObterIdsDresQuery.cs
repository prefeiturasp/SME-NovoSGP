using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsDresQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsDresQuery()
        {
        }
    }
}

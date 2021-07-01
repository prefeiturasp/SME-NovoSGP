using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosUEsQuery : IRequest<IEnumerable<string>>
    {
    }
}

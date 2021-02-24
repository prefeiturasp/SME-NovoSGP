using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosItineranciaQuery : IRequest<IEnumerable<int>>
    {
    }
}

using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosComunicadoQuery : IRequest<IEnumerable<int>>
    {
    }
}

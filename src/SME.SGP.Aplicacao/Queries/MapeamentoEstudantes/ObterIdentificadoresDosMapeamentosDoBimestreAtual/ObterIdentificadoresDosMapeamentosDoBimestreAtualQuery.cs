using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdentificadoresDosMapeamentosDoBimestreAtualQuery : IRequest<IEnumerable<long>>
    {
    }
}

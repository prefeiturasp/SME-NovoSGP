using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosUEsQuery : IRequest<IEnumerable<string>>
    {
        private static ObterCodigosUEsQuery _instance;
        public static ObterCodigosUEsQuery Instance => _instance ??= new();
    }
}

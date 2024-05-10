using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCiclosEolQuery : IRequest<IEnumerable<CicloRetornoDto>>
    {
        private static ObterCiclosEolQuery _instance;
        public static ObterCiclosEolQuery Instance => _instance ??= new();
    }
}

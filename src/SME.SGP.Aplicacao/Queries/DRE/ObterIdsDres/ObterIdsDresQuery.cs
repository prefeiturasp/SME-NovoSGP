using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsDresQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsDresQuery()
        {}

        private static ObterIdsDresQuery _instance;
        public static ObterIdsDresQuery Instance => _instance ??= new();
    }
}

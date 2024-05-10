using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTodasUesIdsQuery : IRequest<IEnumerable<long>>
    {
        public ObterTodasUesIdsQuery()
        {}

        private static ObterTodasUesIdsQuery _instance;
        public static ObterTodasUesIdsQuery Instance => _instance ??= new();
    }
}

using System.Collections.Generic;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterBucketsServicoArmazenamentoQuery: IRequest<IEnumerable<string>>
    {
        public ObterBucketsServicoArmazenamentoQuery()
        {}

        private static ObterBucketsServicoArmazenamentoQuery _instance;
        public static ObterBucketsServicoArmazenamentoQuery Instance => _instance ??= new();
    }
}

using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConceitosValoresQuery : IRequest<IEnumerable<Conceito>>
    {
        private static ObterConceitosValoresQuery _instance;
        public static ObterConceitosValoresQuery Instance => _instance ??= new();
    }
}

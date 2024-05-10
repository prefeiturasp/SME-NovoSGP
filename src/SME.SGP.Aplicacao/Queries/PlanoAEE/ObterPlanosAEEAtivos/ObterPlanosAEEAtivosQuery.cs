using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEAtivosQuery : IRequest<IEnumerable<PlanoAEE>>
    {
        public ObterPlanosAEEAtivosQuery()
        {}

        private static ObterPlanosAEEAtivosQuery _instance;
        public static ObterPlanosAEEAtivosQuery Instance => _instance ??= new();
    }
}

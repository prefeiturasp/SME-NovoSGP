using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDresQuery : IRequest<IEnumerable<DreRespostaEolDto>>
    {
        private static ObterDresQuery _instance;
        public static ObterDresQuery Instance => _instance ??= new();
    }
}

using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDresQuery : IRequest<IEnumerable<DreRespostaEolDto>>
    {
        private static ObterDresQuery _instance;
        public static ObterDresQuery Instance => _instance ??= new();
    }
}

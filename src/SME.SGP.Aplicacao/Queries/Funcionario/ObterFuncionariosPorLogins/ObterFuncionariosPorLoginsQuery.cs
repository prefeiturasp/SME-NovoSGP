using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorLoginsQuery : IRequest<IEnumerable<FuncionarioUnidadeDto>>
    {
        public ObterFuncionariosPorLoginsQuery(IEnumerable<string> logins)
        {
            Logins = logins;
        }

        public IEnumerable<string> Logins { get; set; }
    }
}

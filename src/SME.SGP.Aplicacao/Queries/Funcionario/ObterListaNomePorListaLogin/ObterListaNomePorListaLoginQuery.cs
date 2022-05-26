using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterListaNomePorListaLoginQuery : IRequest<IEnumerable<FuncionarioUnidadeDto>>
    {
        public ObterListaNomePorListaLoginQuery(IEnumerable<string> logins)
        {
            Logins = logins;
        }

        public IEnumerable<string> Logins { get; set; }
    }
}

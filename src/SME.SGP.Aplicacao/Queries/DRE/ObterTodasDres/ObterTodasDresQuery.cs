using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTodasDresQuery : IRequest<IEnumerable<Dre>>
    {
        public ObterTodasDresQuery()
        {
        }
    }
}

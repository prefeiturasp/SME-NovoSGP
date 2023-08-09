using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterLoginAtualQuery : IRequest<string>
    {
        //Como tem chamadas sem parametros seria interessante disponibilizar a mesma instancia sempre para nao precisar fazer alocacao
        public static ObterLoginAtualQuery Instance => new();
    }
}

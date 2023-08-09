using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPerfilAtualQuery : IRequest<Guid>
    {
        //Como tem chamadas sem parametros seria interessante disponibilizar a mesma instancia sempre para nao precisar fazer alocacao
        public static ObterPerfilAtualQuery Instance => new();
    }
}

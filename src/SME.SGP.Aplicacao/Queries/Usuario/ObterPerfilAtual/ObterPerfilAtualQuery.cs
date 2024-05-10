using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPerfilAtualQuery : IRequest<Guid>
    {
        private static ObterPerfilAtualQuery _instance;
        public static ObterPerfilAtualQuery Instance => _instance ??= new();
    }
}

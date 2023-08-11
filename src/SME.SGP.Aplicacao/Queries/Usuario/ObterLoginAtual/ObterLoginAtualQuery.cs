using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterLoginAtualQuery : IRequest<string>
    {
        private static ObterLoginAtualQuery _instance;
        public static ObterLoginAtualQuery Instance => _instance ??= new();
    }
}

using System;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterPerfilDoTokenQuery : IRequest<Guid>
    {
        private static ObterPerfilDoTokenQuery _instance;
        public static ObterPerfilDoTokenQuery Instance => _instance ??= new();
    }    
}
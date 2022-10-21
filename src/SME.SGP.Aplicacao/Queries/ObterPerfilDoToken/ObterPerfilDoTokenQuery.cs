using System;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterPerfilDoTokenQuery : IRequest<Guid>{}    
}
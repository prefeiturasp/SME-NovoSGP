using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.UE.ObterTodasUes
{
    public class ObterTodasUesQuery : IRequest<IEnumerable<Ue>>
    {
    }
}

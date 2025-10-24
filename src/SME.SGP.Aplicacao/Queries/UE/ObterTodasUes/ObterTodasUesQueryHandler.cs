using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.UE.ObterTodasUes
{
    public class ObterTodasUesQueryHandler : IRequestHandler<ObterTodasUesQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterTodasUesQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe;
        }

        public async Task<IEnumerable<Ue>> Handle(ObterTodasUesQuery request, CancellationToken cancellationToken)
        {
            return repositorioUe.ObterTodas();
        }
    }
}

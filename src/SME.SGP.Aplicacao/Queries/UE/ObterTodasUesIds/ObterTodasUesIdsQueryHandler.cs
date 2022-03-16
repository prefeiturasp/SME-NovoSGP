using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTodasUesIdsQueryHandler : IRequestHandler<ObterTodasUesIdsQuery, IEnumerable<long>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterTodasUesIdsQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public Task<IEnumerable<long>> Handle(ObterTodasUesIdsQuery request, CancellationToken cancellationToken)
            => repositorioUe.ObterTodosIds();
    }
}

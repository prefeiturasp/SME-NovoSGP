using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConceitosPorIdsQueryHandler : IRequestHandler<ObterConceitosPorIdsQuery, IEnumerable<Conceito>>
    {
        private readonly IRepositorioConceito repositorioConceito;

        public ObterConceitosPorIdsQueryHandler(IRepositorioConceito repositorioConceito)
        {
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
        }

        public async Task<IEnumerable<Conceito>> Handle(ObterConceitosPorIdsQuery request, CancellationToken cancellationToken)
            => await repositorioConceito.ObterPorIds(request.Ids);
    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosUEsQueryHandler : IRequestHandler<ObterCodigosUEsQuery, IEnumerable<string>>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterCodigosUEsQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<string>> Handle(ObterCodigosUEsQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterCodigosUEs();
    }
}

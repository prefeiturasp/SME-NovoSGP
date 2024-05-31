using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosUePorModalidadeQueryHandler : IRequestHandler<ObterCodigosUePorModalidadeQuery, IEnumerable<string>>
    {
        private readonly IRepositorioUe repositorioUe;
        public ObterCodigosUePorModalidadeQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public Task<IEnumerable<string>> Handle(ObterCodigosUePorModalidadeQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioUe.ObterCodigoUePorModalidade(request.CodigosUe, request.Modalidades);
        }
    }
}

using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEIdPorCodigoQueryHandler : IRequestHandler<ObterUEIdPorCodigoQuery, long>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUEIdPorCodigoQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<long> Handle(ObterUEIdPorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterIdPorCodigo(request.UeCodigo);
    }
}

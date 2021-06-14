using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoEscolaPorCodigoUEQueryHandler : IRequestHandler<ObterTipoEscolaPorCodigoUEQuery, TipoEscola>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterTipoEscolaPorCodigoUEQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<TipoEscola> Handle(ObterTipoEscolaPorCodigoUEQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterTipoEscolaPorCodigo(request.UeCodigo);
    }
}

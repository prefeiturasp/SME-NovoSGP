using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUeComDrePorCodigoQueryHandler : IRequestHandler<ObterUeComDrePorCodigoQuery, Ue>
    {
        private readonly IRepositorioUeConsulta repositorioUe;
        private readonly IRepositorioCache repositorioCache;

        public ObterUeComDrePorCodigoQueryHandler(IRepositorioUeConsulta repositorioUe,IRepositorioCache repositorioCache)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<Ue> Handle(ObterUeComDrePorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterAsync($"codigo-ue-${request.UeCodigo}", async () => await repositorioUe.ObterUeComDrePorCodigo(request.UeCodigo));
        }
    }
}

using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUePorIdQueryHandler : IRequestHandler<ObterUePorIdQuery, Ue>
    {
        private readonly IRepositorioUeConsulta repositorioUe;
        private readonly IRepositorioCache repositorioCache;


        public ObterUePorIdQueryHandler(IRepositorioUeConsulta repositorioUe, IRepositorioCache repositorioCache)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<Ue> Handle(ObterUePorIdQuery request, CancellationToken cancellationToken)
            => await repositorioCache.ObterAsync($"ObterUePorId-{request.Id}",
                async () => await repositorioUe.ObterUePorId(request.Id));
    }
}
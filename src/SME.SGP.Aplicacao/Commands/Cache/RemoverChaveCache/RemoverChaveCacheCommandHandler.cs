using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverChaveCacheCommandHandler : IRequestHandler<RemoverChaveCacheCommand>
    {
        private readonly IRepositorioCache repositorioCache;

        public RemoverChaveCacheCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<Unit> Handle(RemoverChaveCacheCommand request, CancellationToken cancellationToken)
        {
            await repositorioCache.RemoverAsync(request.Chave);
            return Unit.Value;
        }
    }
}

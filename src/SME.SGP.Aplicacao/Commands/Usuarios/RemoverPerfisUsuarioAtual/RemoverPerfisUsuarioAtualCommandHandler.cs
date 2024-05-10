using MediatR;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverPerfisUsuarioAtualCommandHandler : IRequestHandler<RemoverPerfisUsuarioAtualCommand>
    {
        private readonly IRepositorioCache repositorioCache;

        public RemoverPerfisUsuarioAtualCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<Unit> Handle(RemoverPerfisUsuarioAtualCommand request, CancellationToken cancellationToken)
        {
            var chaveCache = string.Format(NomeChaveCache.PERFIS_USUARIO, request.Login);

            await repositorioCache.RemoverAsync(chaveCache);

            return Unit.Value;
        }
    }
}

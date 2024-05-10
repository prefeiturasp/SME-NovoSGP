using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUeComDrePorIdQueryHandler : IRequestHandler<ObterUeComDrePorIdQuery, Ue>
    {
        private readonly IRepositorioUeConsulta repositorioUe;
        private readonly IRepositorioCache repositorioCache;

        public ObterUeComDrePorIdQueryHandler(IRepositorioUeConsulta repositorioUe, IRepositorioCache repositorioCache)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<Ue> Handle(ObterUeComDrePorIdQuery request, CancellationToken cancellationToken)
        {
            var nomeChave = string.Format(NomeChaveCache.UE_ID, request.UeId);
            return await repositorioCache.ObterAsync(nomeChave, 
                async () => await repositorioUe.ObterUeComDrePorId(request.UeId),
                "Obter UE");
        }
    }
}
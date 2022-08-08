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
        private readonly IMediator mediator;


        public ObterUeComDrePorIdQueryHandler(IRepositorioUeConsulta repositorioUe, IMediator mediator)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Ue> Handle(ObterUeComDrePorIdQuery request, CancellationToken cancellationToken)
        {
            var nomeChave = string.Format(NomeChaveCache.CHAVE_UE_COM_DRE_ID, request.UeId);
            return await mediator.Send(new ObterCacheObjetoQuery<Ue>(nomeChave, async () => await repositorioUe.ObterUeComDrePorId(request.UeId)), cancellationToken);
        }
    }
}
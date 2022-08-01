using MediatR;
using SME.SGP.Dominio;
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


        public ObterUeComDrePorIdQueryHandler(IRepositorioUeConsulta repositorioUe,IMediator mediator)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Ue> Handle(ObterUeComDrePorIdQuery request, CancellationToken cancellationToken)
        {
            return await mediator.Send(new ObterCacheObjetoQuery<Ue>($"UeComDre-PorId-{request.UeId}", async () => await repositorioUe.ObterUeComDrePorId(request.UeId)), cancellationToken);
        }
    }
}
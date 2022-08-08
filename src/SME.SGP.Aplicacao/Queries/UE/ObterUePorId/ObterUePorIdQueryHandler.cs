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
        private readonly IMediator mediator;


        public ObterUePorIdQueryHandler(IRepositorioUeConsulta repositorioUe, IMediator mediator)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Ue> Handle(ObterUePorIdQuery request, CancellationToken cancellationToken)
        {
            return await mediator.Send(new ObterCacheObjetoQuery<Ue>($"ObterUePorId-{request.Id}", async () => await repositorioUe.ObterUePorId(request.Id)), cancellationToken);
        }
    }
}
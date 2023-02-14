using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUeComDrePorCodigoQueryHandler : IRequestHandler<ObterUeComDrePorCodigoQuery, Ue>
    {
        private readonly IRepositorioUeConsulta repositorioUe;
        private readonly IMediator mediator;

        public ObterUeComDrePorCodigoQueryHandler(IRepositorioUeConsulta repositorioUe, IMediator mediator)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Ue> Handle(ObterUeComDrePorCodigoQuery request, CancellationToken cancellationToken)
        {
            var ueId = await repositorioUe.ObterIdPorCodigoUe(request.UeCodigo);

            if (ueId > 0)
                return await mediator.Send(new ObterUeComDrePorIdQuery(ueId));

            return null;
        }
    }
}
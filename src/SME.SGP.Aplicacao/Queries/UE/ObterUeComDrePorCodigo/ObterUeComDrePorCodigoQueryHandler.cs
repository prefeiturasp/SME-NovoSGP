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
            var ue = await repositorioUe.ObterUeComDrePorCodigo(request.UeCodigo);
            var nomeChave = string.Format(NomeChaveCache.CHAVE_DRE_ID, ue.Id);
            var dados = JsonConvert.DeserializeObject<Ue>(await mediator.Send(new ObterCacheAsyncQuery(nomeChave)));

            if (dados != null)
                return dados;

            return await mediator.Send(new ObterUeComDrePorIdQuery(ue.Id), cancellationToken);
        }
    }
}
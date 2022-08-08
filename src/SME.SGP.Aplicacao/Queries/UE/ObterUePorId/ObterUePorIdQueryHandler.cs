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
            var nomeChave = string.Format(NomeChaveCache.CHAVE_UE_COM_DRE_ID, request.Id);
            var dados = JsonConvert.DeserializeObject<Ue>(await mediator.Send(new ObterCacheAsyncQuery(nomeChave)));

            if (dados != null)
                return dados;

            return await mediator.Send(new ObterUeComDrePorIdQuery(request.Id), cancellationToken);
        }
    }
}
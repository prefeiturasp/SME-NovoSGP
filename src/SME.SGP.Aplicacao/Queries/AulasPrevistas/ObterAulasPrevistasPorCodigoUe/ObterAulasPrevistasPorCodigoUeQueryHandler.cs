using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPrevistasPorCodigoUeQueryHandler : IRequestHandler<ObterAulasPrevistasPorCodigoUeQuery, IEnumerable<AulaPrevista>>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IMediator mediator;
        private readonly IRepositorioAulaPrevistaConsulta repositorioAulaPrevistaConsulta;
        public ObterAulasPrevistasPorCodigoUeQueryHandler(IRepositorioCache repositorioCache, IMediator mediator, IRepositorioAulaPrevistaConsulta repositorioAulaPrevistaConsulta)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioAulaPrevistaConsulta = repositorioAulaPrevistaConsulta ?? throw new System.ArgumentNullException(nameof(repositorioAulaPrevistaConsulta));
        }

        public async Task<IEnumerable<AulaPrevista>> Handle(ObterAulasPrevistasPorCodigoUeQuery request, CancellationToken cancellationToken)
        {
            if (request.ObterPorCache)
            {
                var nomeChave = string.Format(NomeChaveCache.AULAS_PREVISTAS_UE, request.CodigoUe);
                var atividadesPrevistasNoCache = await repositorioCache.ObterAsync(nomeChave);

                if (string.IsNullOrEmpty(atividadesPrevistasNoCache))
                {
                    return await mediator.Send(new CriarCacheAulaPrevistaCommand(nomeChave, request.CodigoUe));
                }

                return JsonConvert.DeserializeObject<IEnumerable<AulaPrevista>>(atividadesPrevistasNoCache);
            }
            else
                return await repositorioAulaPrevistaConsulta.ObterAulasPrevistasPorUe(request.CodigoUe);

        }


    }
}

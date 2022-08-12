﻿using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
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
        public ObterAulasPrevistasPorCodigoUeQueryHandler(IRepositorioCache repositorioCache, IMediator mediator)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AulaPrevista>> Handle(ObterAulasPrevistasPorCodigoUeQuery request, CancellationToken cancellationToken)
        {
            var nomeChave = $"Aulas-Previstas-{request.CodigoUe}";
            var atividadesPrevistasNoCache = await repositorioCache.ObterAsync(nomeChave);

            if (string.IsNullOrEmpty(atividadesPrevistasNoCache))
            {
                return await mediator.Send(new CriarCacheAulaPrevistaCommand(nomeChave, request.CodigoUe));
            }

            return JsonConvert.DeserializeObject<IEnumerable<AulaPrevista>>(atividadesPrevistasNoCache);

        }


    }
}

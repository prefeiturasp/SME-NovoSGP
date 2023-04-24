﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake
{
    public class ObterTodasUesIdsQueryHandlerFake : IRequestHandler<ObterTodasUesIdsQuery, IEnumerable<long>>
    {
        public async Task<IEnumerable<long>> Handle(ObterTodasUesIdsQuery request, CancellationToken cancellationToken)
        {
            var listIds = new List<long>()
            {
                11,1,2
            };

            return listIds;
        }
    }
}
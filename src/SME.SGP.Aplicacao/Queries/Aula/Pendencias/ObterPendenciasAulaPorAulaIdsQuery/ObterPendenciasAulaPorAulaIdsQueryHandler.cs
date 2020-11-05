﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdsQueryHandler : IRequestHandler<ObterPendenciasAulaPorAulaIdsQuery, long[]>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public ObterPendenciasAulaPorAulaIdsQueryHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }
        public async Task<long[]> Handle(ObterPendenciasAulaPorAulaIdsQuery request, CancellationToken cancellationToken)
        {
            var valor = await repositorioPendenciaAula.ListarPendenciasPorAulasId(request.AulasId);

            return valor;
        }
    }
}

﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdsQueryHandler : IRequestHandler<ObterPendenciasAulaPorAulaIdsQuery, bool>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;
        public ObterPendenciasAulaPorAulaIdsQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }
        public async Task<bool> Handle(ObterPendenciasAulaPorAulaIdsQuery request, CancellationToken cancellationToken)
        {
            var possuiPendencia = await repositorioPendenciaAula.PossuiPendenciasPorAulasId(request.AulasId, request.EhModalidadeInfantil, request.EtapaEJA, request.ComponentesId);
            if (!possuiPendencia)
                possuiPendencia = await repositorioPendenciaAula.PossuiAtividadeAvaliativaSemNotaPorAulasId(request.AulasId);
            return possuiPendencia;
        }
    }
}

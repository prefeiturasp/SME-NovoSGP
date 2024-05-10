﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQueryHandler : IRequestHandler<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery, bool>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;

        public VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<bool> Handle(VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery request, CancellationToken cancellationToken)
        {
            var planoAEE = await repositorioPlanoAEE.ObterPlanoPorEstudanteEAno(request.CodigoEstudante, request.AnoLetivo);
            if (planoAEE.EhNulo())
                return false;

            return true;
        }
    }
}


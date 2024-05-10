﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PeriodoEscolar.PeriodoEscolarEmAberto
{
    public class PeriodoEscolarEmAbertoQueryHandler : IRequestHandler<PeriodoEscolarEmAbertoQuery, bool>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public PeriodoEscolarEmAbertoQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<bool> Handle(PeriodoEscolarEmAbertoQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoEscolar.PeriodoEmAbertoAsync(request.TipoCalendarioId, request.DataReferencia, request.Bimestre, request.EhAnoLetivo);
    }
}

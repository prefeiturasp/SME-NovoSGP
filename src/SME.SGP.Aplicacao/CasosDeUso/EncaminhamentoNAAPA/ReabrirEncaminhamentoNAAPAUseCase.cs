﻿using MediatR;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReabrirEncaminhamentoNAAPAUseCase : IReabrirEncaminhamentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ReabrirEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<SituacaoDto> Executar(long encaminhamentoId)
        {
            return await mediator.Send(new ReabrirEncaminhamentoNAAPACommand(encaminhamentoId));
        }
    }
}

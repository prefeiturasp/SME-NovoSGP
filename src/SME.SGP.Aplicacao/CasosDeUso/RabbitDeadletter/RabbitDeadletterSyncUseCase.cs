﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RabbitDeadletterSyncUseCase : IRabbitDeadletterSyncUseCase
    {
        private readonly IMediator mediator;

        public RabbitDeadletterSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar()
        {
            foreach (var fila in typeof(RotasRabbitSgp).ObterConstantesPublicas<string>())
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaRabbitDeadletterTratar, fila, Guid.NewGuid(), null, false));
            }

            return true;
        }
    }
}

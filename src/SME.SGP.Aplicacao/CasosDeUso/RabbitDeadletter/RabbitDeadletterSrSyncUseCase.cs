using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RabbitDeadletterSrSyncUseCase : IRabbitDeadletterSrSyncUseCase
    {
        private readonly IMediator mediator;

        public RabbitDeadletterSrSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar()
        {
            foreach (var fila in typeof(RotasRabbitSgpRelatorios).ObterConstantesPublicas<string>())
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaRabbitSRDeadletterTratar, fila, Guid.NewGuid(), null, false));
            }

            return true;
        }
    }
}

using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaTrataNotificacoesNiveisCargosUseCase : IExecutaTrataNotificacoesNiveisCargosUseCase
    {
        private readonly IMediator mediator;

        public ExecutaTrataNotificacoesNiveisCargosUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem TrataNotificacoesNiveisCargosUseCase", "Rabbit - TrataNotificacoesNiveisCargosUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaTrataNotificacoesNiveis, new TrataNotificacoesNiveisCargosUseCase(mediator), Guid.NewGuid(), null));
        }
    }
}

using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoResultadoInsatisfatorioUseCase : AbstractUseCase, IExecutaNotificacaoResultadoInsatisfatorioUseCase
    {
        public ExecutaNotificacaoResultadoInsatisfatorioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem NotificacaoResultadoInsatisfatorioUseCase", "Rabbit - NotificacaoResultadoInsatisfatorioUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoResultadoInsatisfatorio, null, Guid.NewGuid(), null));
        }
    }
}

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
    public class ExecutaNotificacaoPeriodoFechamentoUseCase : AbstractUseCase, IExecutaNotificacaoPeriodoFechamentoUseCase
    {
        public ExecutaNotificacaoPeriodoFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem NotificacaoPeriodoFechamento", "Rabbit - NotificacaoPeriodoFechamento");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoPeriodoFechamento, null, Guid.NewGuid(), null));
        }
    }
}

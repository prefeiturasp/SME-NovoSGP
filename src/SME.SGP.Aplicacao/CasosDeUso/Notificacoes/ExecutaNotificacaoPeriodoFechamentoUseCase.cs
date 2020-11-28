using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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
            SentrySdk.AddBreadcrumb($"Mensagem NotificacaoPeriodoFechamentoUseCase", "Rabbit - NotificacaoPeriodoFechamentoUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaNotificacaoPeriodoFechamento, null, Guid.NewGuid(), null));
        }
    }
}

using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoInicioFimPeriodoFechamentoUseCase : AbstractUseCase, IExecutaNotificacaoInicioFimPeriodoFechamentoUseCase
    {
        public ExecutaNotificacaoInicioFimPeriodoFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem NotificacaoInicioFimPeriodoFechamento", "Rabbit - NotificacaoInicioFimPeriodoFechamento");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoInicioFimPeriodoFechamento, null, Guid.NewGuid(), null));
        }
    }
}

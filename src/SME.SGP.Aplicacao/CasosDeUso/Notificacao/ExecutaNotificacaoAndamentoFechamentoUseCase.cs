using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoAndamentoFechamentoUseCase : AbstractUseCase, IExecutaNotificacaoAndamentoFechamentoUseCase
    {
        public ExecutaNotificacaoAndamentoFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem NotificacaoAndamentoFechamentoUseCase", "Rabbit - NotificacaoAndamentoFechamentoUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoAndamentoFechamento, null, Guid.NewGuid(), null));
        }
    }
}

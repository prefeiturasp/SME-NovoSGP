using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoUeFechamentosInsuficientesUseCase : AbstractUseCase, IExecutaNotificacaoUeFechamentosInsuficientesUseCase
    {
        public ExecutaNotificacaoUeFechamentosInsuficientesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem NotificacaoUeFechamentosInsuficientesUseCase", "Rabbit - NotificacaoUeFechamentosInsuficientesUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoUeFechamentosInsuficientes, null, Guid.NewGuid(), null));
        }
    }
}

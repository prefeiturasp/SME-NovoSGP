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
    public class ExecutaNotificacaoPlanoAEEEmAbertoUseCase : AbstractUseCase, IExecutaNotificacaoPlanoAEEEmAbertoUseCase
    {
        public ExecutaNotificacaoPlanoAEEEmAbertoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutaNotificacaoPlanoAEEEmAbertoUseCase", "Rabbit - ExecutaNotificacaoPlanoAEEEmAbertoUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.NotificarPlanoAEEEmAberto, null, Guid.NewGuid(), null));
        }
    }
}

using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoPlanoAEEExpiradoUseCase : AbstractUseCase, IExecutaNotificacaoPlanoAEEExpiradoUseCase
    {
        public ExecutaNotificacaoPlanoAEEExpiradoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutaNotificacaoPlanoAEEExpiradoUseCase", "Rabbit - ExecutaNotificacaoPlanoAEEExpiradoUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.NotificarPlanoAEEExpirado, null, Guid.NewGuid(), null));
        }
    }
}

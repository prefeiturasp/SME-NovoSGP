using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoFrequenciaUeUseCase : AbstractUseCase, IExecutaNotificacaoFrequenciaUeUseCase
    {
        public ExecutaNotificacaoFrequenciaUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb("Mensagem NotificacaoFrequenciaUe", "Rabbit - NotificacaoFrequenciaUe");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoFrequenciaUe, null, Guid.NewGuid(), null));
        }
    }
}

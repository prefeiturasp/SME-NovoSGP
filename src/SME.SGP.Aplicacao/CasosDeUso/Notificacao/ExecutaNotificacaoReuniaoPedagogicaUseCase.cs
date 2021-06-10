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
    public class ExecutaNotificacaoReuniaoPedagogicaUseCase : AbstractUseCase, IExecutaNotificacaoReuniaoPedagogicaUseCase
    {
        public ExecutaNotificacaoReuniaoPedagogicaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem NotificacaoReuniaoPedagogica", "Rabbit - NotificacaoReuniaoPedagogica");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoReuniaoPedagogica, null, Guid.NewGuid(), null));
        }
    }
}

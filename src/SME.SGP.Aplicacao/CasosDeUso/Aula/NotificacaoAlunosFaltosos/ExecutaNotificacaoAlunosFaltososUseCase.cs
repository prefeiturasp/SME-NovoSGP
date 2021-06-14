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
    public class ExecutaNotificacaoAlunosFaltososUseCase : AbstractUseCase, IExecutaNotificacaoAlunosFaltososUseCase
    {
        public ExecutaNotificacaoAlunosFaltososUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb("Mensagem NotificacaoAlunosFaltosos", "Rabbit - NotificacaoAlunosFaltosos");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoAlunosFaltosos, null, Guid.NewGuid(), null));
        }
    }
}

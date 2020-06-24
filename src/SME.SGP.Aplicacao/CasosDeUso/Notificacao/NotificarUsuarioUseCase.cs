using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Sentry;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class NotificarUsuarioUseCase : AbstractUseCase, INotificarUsuarioUseCase
    {
        public NotificarUsuarioUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem NotificarUsuarioUseCase", "Rabbit - NotificarUsuarioUseCase");

            var command = mensagemRabbit.ObterObjetoFiltro<NotificarUsuarioCommand>();

            return await mediator.Send(command);
        }
    }
}

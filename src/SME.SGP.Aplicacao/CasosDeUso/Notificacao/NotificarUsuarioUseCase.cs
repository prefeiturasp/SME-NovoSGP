﻿using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarUsuarioUseCase : AbstractUseCase, INotificarUsuarioUseCase
    {
        public NotificarUsuarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<long> Executar(MensagemRabbit mensagemRabbit)
        {
            var command = mensagemRabbit.ObterObjetoMensagem<NotificarUsuarioCommand>();

            return await mediator.Send(command);
        }
    }
}

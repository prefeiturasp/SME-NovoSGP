using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoPendenciasAulaUseCase : AbstractUseCase, IExecutarExclusaoPendenciasAulaUseCase
    {
        public ExecutarExclusaoPendenciasAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var command = mensagem.ObterObjetoMensagem<ExcluirTodasPendenciasAulaCommand>();

            await mediator.Send(command);
            return true;
        }
    }
}

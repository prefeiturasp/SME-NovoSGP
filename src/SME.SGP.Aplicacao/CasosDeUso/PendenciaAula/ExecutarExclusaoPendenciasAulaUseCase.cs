using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Sentry;
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

            LogSentry(command, "Inicio");
            await mediator.Send(command);
            LogSentry(command, "Fim");

            return true;
        }

        private void LogSentry(ExcluirTodasPendenciasAulaCommand command, string mensagem)
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutarExclusaoPendenciasAulaUseCase : {mensagem} - Aula:{command.AulaId}", "Rabbit - ExecutarExclusaoPendenciasAulaUseCase");
        }
    }
}

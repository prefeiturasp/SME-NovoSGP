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
            var filtro = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            await mediator.Send(new ExcluirTodasPendenciasAulaCommand(filtro.Id));
            return true;
        }

        private void LogSentry(ExcluirTodasPendenciasAulaCommand command, string mensagem)
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutarExclusaoPendenciasAulaUseCase : {mensagem} - Aula:{command.AulaId}", "Rabbit - ExecutarExclusaoPendenciasAulaUseCase");
        }
    }
}

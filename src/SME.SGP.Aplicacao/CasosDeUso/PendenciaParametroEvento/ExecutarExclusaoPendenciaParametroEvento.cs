using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Sentry;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoPendenciaParametroEvento : AbstractUseCase, IExecutarExclusaoPendenciaParametroEvento
    {
        public ExecutarExclusaoPendenciaParametroEvento(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var command = mensagem.ObterObjetoMensagem<VerificaExclusaoPendenciasParametroEventoCommand>();

            LogSentry(command, "Inicio");
            await mediator.Send(command);
            LogSentry(command, "Fim");

            return true;
        }

        private void LogSentry(VerificaExclusaoPendenciasParametroEventoCommand command, string mensagem)
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutarExclusaoPendenciaParametroEvento : {mensagem} - Ue:{command.UeCodigo} TipoEvento:{command.TipoEvento}", "Rabbit - ExecutarExclusaoPendenciaParametroEvento");
        }
    }
}

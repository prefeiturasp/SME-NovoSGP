using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Sentry;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoPendenciasAusenciaFechamentoUseCase : AbstractUseCase, IExecutarExclusaoPendenciasAusenciaFechamentoUseCase
    {
        public ExecutarExclusaoPendenciasAusenciaFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var command = mensagem.ObterObjetoMensagem<VerificaExclusaoPendenciasAusenciaFechamentoCommand>();

            LogSentry(command, "Inicio");
            await mediator.Send(command);
            LogSentry(command, "Fim");

            return true;
        }

        private void LogSentry(VerificaExclusaoPendenciasAusenciaFechamentoCommand command, string mensagem)
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutarExclusaoPendenciasAusenciaFechamentoUseCase : {mensagem} - Turma:{command.TurmaId} Tipo: AusenciaFechamento", "Rabbit - ExecutarExclusaoPendenciasAusenciaFechamentoUseCase");
        }
    }
}

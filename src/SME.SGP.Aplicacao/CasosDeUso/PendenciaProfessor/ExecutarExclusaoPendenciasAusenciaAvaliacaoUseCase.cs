using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Sentry;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase : AbstractUseCase, IExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase
    {
        public ExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var command = mensagem.ObterObjetoMensagem<VerificaExclusaoPendenciasAusenciaAvaliacaoCommand>();

            LogSentry(command, "Inicio");
            await mediator.Send(command);
            LogSentry(command, "Fim");

            return true;
        }

        private void LogSentry(VerificaExclusaoPendenciasAusenciaAvaliacaoCommand command, string mensagem)
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutarExclusaoPendenciasAusenciaAvaliacao : {mensagem} - Turma:{command.TurmaCodigo} Tipo:{command.TipoPendencia.ToString()}", "Rabbit - ExecutarExclusaoPendenciasAusenciaAvaliacao");
        }
    }
}

using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoPendenciaDiasLetivosInsuficientes : AbstractUseCase, IExecutarExclusaoPendenciaDiasLetivosInsuficientes
    {
        public ExecutarExclusaoPendenciaDiasLetivosInsuficientes(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var command = mensagem.ObterObjetoMensagem<ExcluirPendenciasDiasLetivosInsuficientesCommand>();

            LogSentry(command, "Inicio");
            await mediator.Send(command);
            LogSentry(command, "Fim");
            
            return true;
        }

        private void LogSentry(ExcluirPendenciasDiasLetivosInsuficientesCommand command, string mensagem)
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutarExclusaoPendenciaDiasLetivosInsuficientes : {mensagem} - TipoCalendario:{command.TipoCalendarioId} Dre:{command.DreCodigo} Ue:{command.UeCodigo}", "Rabbit - ExecutarExclusaoPendenciaDiasLetivosInsuficientes");
        }
    }
}

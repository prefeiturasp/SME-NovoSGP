using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciasFechamentoUseCase : AbstractUseCase, IGerarPendenciasFechamentoUseCase
    {
        public GerarPendenciasFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var command = mensagem.ObterObjetoMensagem<GerarPendenciasFechamentoCommand>();

            LogSentry(command, "Inicio");
            await mediator.Send(command);
            LogSentry(command, "Fim");

            return true;
        }

        private void LogSentry(GerarPendenciasFechamentoCommand command, string mensagem)
        {
            SentrySdk.AddBreadcrumb($"Mensagem GerarPendenciasFechamentoUseCase : {mensagem} - Turma:{command.TurmaCodigo} Componente:{command.ComponenteCurricularId} Bimestre:{command.Bimestre} ", "Rabbit - GerarPendenciasFechamentoUseCase");
        }
    }
}

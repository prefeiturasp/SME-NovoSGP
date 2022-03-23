using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

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

            await mediator.Send(command);

            return true;
        }
    }
}

using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

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

            await mediator.Send(command);

            return true;
        }

    }
}

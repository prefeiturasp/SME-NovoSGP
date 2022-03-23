using MediatR;
using SME.SGP.Infra;
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

            await mediator.Send(command);

            return true;
        }
    }
}

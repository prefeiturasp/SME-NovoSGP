using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

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
            
            await mediator.Send(command);            

            return true;
        }
    }
}

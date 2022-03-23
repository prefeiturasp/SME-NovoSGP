using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

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
    }
}

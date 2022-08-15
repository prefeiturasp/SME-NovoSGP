using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoPendenciasDevolutivaUseCase : AbstractUseCase, IExecutarExclusaoPendenciasDevolutivaUseCase
    {
        public ExecutarExclusaoPendenciasDevolutivaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroExclusaoPendenciasDevolutivaDto>();
            await mediator.Send(new ExcluirPendenciasDevolutivaPorTurmaEComponenteCommand(filtro.TurmaId, filtro.ComponenteId));
            return await Task.FromResult(true);
        }
    }
}

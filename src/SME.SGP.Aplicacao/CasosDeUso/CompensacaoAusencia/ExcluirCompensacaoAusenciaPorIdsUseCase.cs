using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaPorIdsUseCase : AbstractUseCase, IExcluirCompensacaoAusenciaPorIdsUseCase
    {
        public ExcluirCompensacaoAusenciaPorIdsUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroCompensacaoAusenciaDto>();
            
            await mediator.Send(new ExcluirCompensacaoAusenciaPorIdsCommand(filtro.CompensacaoAusenciaIds));
            
            return true;
        }
    }
}

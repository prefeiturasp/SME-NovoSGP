using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaDiarioBordoParaExcluirUseCase : AbstractUseCase, IPendenciaDiarioBordoParaExcluirUseCase
    {
        public PendenciaDiarioBordoParaExcluirUseCase(IMediator mediator) : base(mediator)
        {            
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroListaAulaIdComponenteCurricularIdDto>();
            var command = new PendenciaDiarioBordoParaExcluirCommand(filtro.PendenciaDiariosBordoParaExcluirDto);
            return await mediator.Send(command);
        }
    }
}

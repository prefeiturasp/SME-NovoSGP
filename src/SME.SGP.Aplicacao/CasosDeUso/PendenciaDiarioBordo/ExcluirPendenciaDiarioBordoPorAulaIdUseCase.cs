using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaDiarioBordoPorAulaIdUseCase : AbstractUseCase, IExcluirPendenciaDiarioBordoPorAulaIdUseCase
    {
        public ExcluirPendenciaDiarioBordoPorAulaIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroIdDto>();
            await mediator.Send(new ExcluirPendenciaDiarioPorAulaIdCommand(filtro.Id));
            return true;
        }
    }
}

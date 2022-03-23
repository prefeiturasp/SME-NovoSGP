using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaVerificacaoPendenciasGeraisCalendarioUseCase : AbstractUseCase, IExecutaVerificacaoPendenciasGeraisCalendarioUseCase
    {
        public ExecutaVerificacaoPendenciasGeraisCalendarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await mediator.Send(new VerificaPendenciaCalendarioUeCommand());
            return true;
        }
    }
}

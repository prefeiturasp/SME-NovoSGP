using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaVerificacaoPendenciasGeraisUseCase : AbstractUseCase, IExecutaVerificacaoPendenciasGeraisUseCase
    {
        public ExecutaVerificacaoPendenciasGeraisUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar(MensagemRabbit mensagem)
        {
            await mediator.Send(new VerificarPendenciaAulaDiasNaoLetivosCommand());
            await mediator.Send(new VerificaPendenciaCalendarioUeCommand());
            await mediator.Send(new VerificaPendenciaParametroEventoCommand());
        }
    }
}

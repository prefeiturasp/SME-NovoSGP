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

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.PendenciasGeraisAulas, null));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.PendenciasGeraisCalendario, null));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.PendenciasGeraisEventos, null));

            return true;
        }
    }
}

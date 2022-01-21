using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciasGeraisUseCase : AbstractUseCase, IPendenciasGeraisUseCase
    {
        public PendenciasGeraisUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaVerificacaoPendenciasGerais, new ExecutaVerificacaoPendenciasGeraisUseCase(mediator), Guid.NewGuid(), null));
            return true;
        }
    }
}

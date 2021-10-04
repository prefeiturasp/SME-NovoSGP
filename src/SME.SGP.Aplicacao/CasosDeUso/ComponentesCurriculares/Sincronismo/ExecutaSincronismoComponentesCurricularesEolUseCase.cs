using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaSincronismoComponentesCurricularesEolUseCase : AbstractUseCase, IExecutaSincronismoComponentesCurricularesEolUseCase
    {
        public ExecutaSincronismoComponentesCurricularesEolUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            //SentrySdk.AddBreadcrumb($"Mensagem ExecutaSincronismoComponentesCurricularesEolUseCase", "Rabbit - ExecutaSincronismoComponentesCurricularesEolUseCase");
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaSincronizaComponetesCurricularesEol, new ExecutarSincronismoComponentesCurricularesUseCase(mediator), Guid.NewGuid(), null));
            return true;
        }

    }
}
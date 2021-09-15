using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaSincronismoComponentesCurricularesEolUseCase:AbstractUseCase, IExecutaSincronismoComponentesCurricularesEolUseCase
    {
        public ExecutaSincronismoComponentesCurricularesEolUseCase(IMediator mediator) : base(mediator)
        {

        }
        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutaSincronismoComponentesCurricularesEolUseCase", "Rabbit - ExecutaSincronismoComponentesCurricularesEolUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaSincronizaComponetesCurricularesEol, new ExecutarSincronismoComponentesCurricularesUseCase(mediator), Guid.NewGuid(), null));
        }
    }
}
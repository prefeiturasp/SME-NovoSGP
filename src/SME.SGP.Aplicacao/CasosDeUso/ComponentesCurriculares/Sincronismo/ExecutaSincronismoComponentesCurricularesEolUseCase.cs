using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaSincronismoComponentesCurricularesEolUseCase: IExecutaSincronismoComponentesCurricularesEolUseCase
    {
        private readonly IMediator mediator;
        private readonly IListarComponentesCurricularesEolUseCase listarComponentesCurricularesEolUseCase;
        public ExecutaSincronismoComponentesCurricularesEolUseCase(IMediator mediator, IListarComponentesCurricularesEolUseCase listarComponentesCurricularesEolUseCase)
        {
            this.listarComponentesCurricularesEolUseCase = listarComponentesCurricularesEolUseCase ??
                throw new System.ArgumentNullException(nameof(listarComponentesCurricularesEolUseCase));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutaSincronismoComponentesCurricularesEolUseCase", "Rabbit - ExecutaSincronismoComponentesCurricularesEolUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaSincronizaComponetesCurricularesEol, new SincronizarComponentesCurricularesUseCase(listarComponentesCurricularesEolUseCase), Guid.NewGuid(), null));
        }
    }
}
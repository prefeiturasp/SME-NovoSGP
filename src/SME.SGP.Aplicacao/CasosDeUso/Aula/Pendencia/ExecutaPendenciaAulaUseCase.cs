using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaPendenciaAulaUseCase : IExecutaPendenciaAulaUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public ExecutaPendenciaAulaUseCase(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Executar()
        {
            //SentrySdk.AddBreadcrumb($"Mensagem ExecutaPendenciaAulaUseCase", "Rabbit - ExecutaPendenciaAulaUseCase");
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAula, new PendenciaAulaUseCase(mediator, unitOfWork), Guid.NewGuid(), null));
        }
    }
}
